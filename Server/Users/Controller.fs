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
                    let users = find UserCriteria.All
                    json users next context
        ]
