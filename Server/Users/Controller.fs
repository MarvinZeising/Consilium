namespace Users

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2
open ControllerHelpers
open Authentication

module UserController =

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
                    context.TryGetRequestHeader "Authorization"
                       |> Option.bind (fun token -> token.Replace("Bearer ", "") |> getEmailFromToken)
                       |> Option.bind find
                        |> resultOrStatusCode 404 next context

            DELETE >=> route "/user" >=> authorize >=>
                fun next context ->
                    let delete = context.GetService<UserDelete>()
                    context.TryGetRequestHeader "Authorization"
                        |> Option.bind (fun token -> token.Replace("Bearer ", "") |> getIdFromToken)
                        |> Option.bind delete
                        |> resultOrStatusCode 500 next context

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
