namespace Consilium

module UserController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2
    open UserTypes
    open Authentication
    open UserActions
    open Controller

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [

            POST >=> route "/authenticate" >=>
                fun next context ->
                    task {
                        let! credentials = context.BindJsonAsync<Credentials>()
                        return! sendString (signIn credentials) next context
                    }

            POST >=> route "/users" >=>
                fun next context ->
                    task {
                        let! credentials = context.BindJsonAsync<Credentials>()
                        return! json (signUp credentials) next context
                    }

            GET >=> route "/users" >=> authorize >=>
                fun next context ->
                    sendJson (getUser context) next context

            DELETE >=> route "/users" >=> authorize >=>
                fun next context ->
                    let result = deleteUser context
                    sendJson result next context

            PUT >=> route "/users/email" >=> authorize >=>
                fun next context ->
                    task {
                        let! request = context.BindJsonAsync<UpdateEmailRequest>()
                        let result = updateEmail context request
                        return! sendJson result next context
                    }

            PUT >=> route "/users/language" >=> authorize >=>
                fun next context ->
                    task {
                        let! request = context.BindJsonAsync<UpdateLanguageRequest>()
                        let result = updateLanguage context request
                        return! sendJson result next context
                    }

            PUT >=> route "/users/password" >=> authorize >=>
                fun next context ->
                    task {
                        let! request = context.BindJsonAsync<UpdatePasswordRequest>()
                        let result = updatePassword context request
                        return! sendJson result next context
                    }

            GET >=> routef "/users/email-available/%s" (fun email ->
                fun next context ->
                    task {
                        let available = email |> isEmailAvailable
                        return! sendJson available next context
                    })

        ]
