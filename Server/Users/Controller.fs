namespace Users

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2
open System

module UserController =

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [

            POST >=> route "/users" >=>
                fun next context ->
                    task {
                        let save = context.GetService<UserSave>()
                        let! user = context.BindJsonAsync<User>()
                        let user = { user with Id = ShortGuid.fromGuid(Guid.NewGuid()) }
                        return! json (save user) next context
                    }

            GET >=> route "/users" >=>
                fun next context ->
                    let find = context.GetService<UserFind>()
                    let username = context.TryGetQueryStringValue "username"
                    let users =
                        match username with
                        | None ->
                            find All
                        | Some username ->
                            find(Username username)
                    json users next context

        ]
