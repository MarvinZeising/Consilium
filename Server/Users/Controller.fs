namespace Users

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2
open ControllerHelpers
open Authentication

module UserController =

    let getFromToken extractor (context: HttpContext) =
        context.TryGetRequestHeader "Authorization"
           |> Option.bind (fun token -> token.Replace("Bearer ", "") |> extractor)

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
                        |> getFromToken extractEmailClaim
                        |> Option.bind find
                        |> resultOrStatusCode 404 next context

            DELETE >=> route "/user" >=> authorize >=>
                fun next context ->
                    let delete = context.GetService<UserDelete>()
                    context
                        |> getFromToken extractIdClaim
                        |> Option.bind delete
                        |> resultOrStatusCode 500 next context

            PUT >=> route "/user/password" >=> authorize >=>
                fun next context ->
                    task {
                        let updatePassword = context.GetService<UpdatePassword>()
                        let! passwordChange = context.BindJsonAsync<PasswordChange>()
                        return! json (updatePassword passwordChange) next context
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
