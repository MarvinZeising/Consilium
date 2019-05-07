namespace Users

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2

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

            GET >=> route "/users" >=>
                fun next context ->
                    let find = context.GetService<UserFind>()
                    let users = find ""
                    json users next context

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
