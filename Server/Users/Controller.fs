namespace Consilium

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2
open CommonTypes
open UserTypes
open Authentication
open UserActions
open Users

module UserController =

    let getFromToken extractor (context: HttpContext) =
        context.TryGetRequestHeader "Authorization"
           |> Option.bind (fun token -> token.Replace("Bearer ", "") |> extractor)

    let send =
        CommonLibrary.either json (fun errors -> mapErrorCode (List.head errors)
                                                 |> setStatusCode
                                                 >=> json (List.map string errors))

    let resultOrStatusCode code next context x =
        match x with
        | Some x -> json x next context
        | None -> setStatusCode code next context

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [

            POST >=> route "/users" >=>
                fun next context ->
                    task {
                        let signUp = context.GetService<SignUp>()
                        let! credentials = context.BindJsonAsync<Credentials>()
                        return! json (signUp credentials) next context
                    }

            GET >=> route "/user" >=> authorize >=>
                fun next context ->
                    let find = context.GetService<UserFind>()
                    context
                        |> getFromToken getEmailFromToken
                        |> Option.bind find
                        |> resultOrStatusCode 404 next context

            DELETE >=> route "/user" >=> authorize >=>
                fun next context ->
                    let delete = context.GetService<UserDelete>()
                    context
                        |> getFromToken getIdFromToken
                        |> Option.bind delete
                        |> resultOrStatusCode 500 next context

            PUT >=> route "/user/email" >=> authorize >=>
                fun next context ->
                    task {
                        let! request = context.BindJsonAsync<UpdateEmailRequest>()
                        let result = updateEmail context request
                        return! send result next context
                    }

            PUT >=> route "/user/language" >=> authorize >=>
                fun next context ->
                    task {
                        let! request = context.BindJsonAsync<UpdateLanguageRequest>()
                        let result = updateLanguage context request
                        return! send result next context
                    }

            PUT >=> route "/user/password" >=> authorize >=>
                fun next context ->
                    task {
                        let! request = context.BindJsonAsync<UpdatePasswordRequest>()
                        let result = updatePassword context request
                        return! send result next context
                    }

            GET >=> routef "/users/email-available/%s" (fun email ->
                fun next context ->
                    let emailAvailable = context.GetService<EmailAvailable>()
                    let available = emailAvailable email
                    json available next context)

            POST >=> route "/authenticate" >=>
                fun next context ->
                    task {
                        let signIn = context.GetService<SignIn>()
                        let! credentials = context.BindJsonAsync<Credentials>()
                        return! json (signIn credentials) next context
                    }

        ]
