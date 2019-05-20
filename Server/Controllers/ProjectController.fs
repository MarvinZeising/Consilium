namespace Consilium

open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2
open System
open Projects
open ProjectTypes
open ProjectActions
open Authentication

module ProjectController =

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [
            POST >=> route "/projects" >=> authorize >=>
                fun next context ->
                    task {
                        let save = context.GetService<ProjectSave>()
                        let! project = context.BindJsonAsync<Projects.Project>()
                        let project = { project with Id = ShortGuid.fromGuid(Guid.NewGuid()) }
                        return! json (save project) next context
                    }

            GET >=> route "/projects" >=> authorize >=>
                fun next context ->
                    let find = context.GetService<ProjectFind>()
                    let projects = find All
                    json projects next context

            GET >=> routef "/projects/%s" (fun id ->
                authorize >=> fun next context ->
                    let findById = context.GetService<ProjectFindById>()
                    let project = findById id
                    json project next context)

            PUT >=> routef "/projects/%s" (fun id ->
                authorize >=> fun next context ->
                    task {
                        let! input = context.BindJsonAsync<UpdateGeneralRequest>()
                        let request = { input with Id = id }
                        return! json (updateGeneral request) next context
                    })

            DELETE >=> routef "/projects/%s" (fun id ->
                authorize >=> fun next context ->
                    let delete = context.GetService<ProjectDelete>()
                    json (delete id) next context)
        ]
