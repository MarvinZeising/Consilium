namespace Projects

open Giraffe
open Microsoft.AspNetCore.Http
open Projects
open FSharp.Control.Tasks.V2
open System

module ProjectController =

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [
            POST >=> route "/projects" >=>
                fun next context ->
                    task {
                        let save = context.GetService<ProjectSave>()
                        let! project = context.BindJsonAsync<Project>()
                        let project = { project with Id = ShortGuid.fromGuid(Guid.NewGuid()) }
                        return! json (save project) next context
                    }

            GET >=> route "/projects" >=>
                fun next context ->
                    let find = context.GetService<ProjectFind>()
                    let projects = find ProjectCriteria.All
                    json projects next context

            GET >=> routef "/projects/%s" (fun id ->
                fun next context ->
                    let findById = context.GetService<ProjectFindById>()
                    let project = findById id
                    json project next context)

            PUT >=> routef "/projects/%s" (fun id ->
                fun next context ->
                    task {
                        let save = context.GetService<ProjectSave>()
                        let! project = context.BindJsonAsync<Project>()
                        let project = { project with Id = id }
                        return! json (save project) next context
                    })

            DELETE >=> routef "/projects/%s" (fun id ->
                fun next context ->
                    let delete = context.GetService<ProjectDelete>()
                    json (delete id) next context)
        ]
