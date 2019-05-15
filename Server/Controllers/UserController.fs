namespace Consilium

module UserController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2
    open UserTypes
    open Authentication
    open UserActions
    open Users
    open Controller

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [

            POST >=> route "/authenticate" >=>
                fun next context ->
                    task {
                        let signIn = context.GetService<SignIn>()
                        let! credentials = context.BindJsonAsync<Credentials>()
                        return! json (signIn credentials) next context
                    }

            POST >=> route "/users" >=>
                fun next context ->
                    task {
                        let signUp = context.GetService<SignUp>()
                        let! credentials = context.BindJsonAsync<Credentials>()
                        return! json (signUp credentials) next context
                    }

            GET >=> route "/user" >=> authorize >=>
                fun next context ->
                    task {
                        let user = getUser context
                        return! send user next context
                    }

            DELETE >=> route "/user" >=> authorize >=>
                fun next context ->
                    let result = deleteUser context
                    send result next context

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
                    task {
                        let available = email |> isEmailAvailable
                        return! send available next context
                    })

        ]
