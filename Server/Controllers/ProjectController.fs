namespace Consilium

module ProjectController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2
    open System
    open Projects
    open ProjectTypes
    open ProjectActions
    open Authentication
    open Controller

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [
            POST >=> route "/projects"
                >=> authorize
                >=> fun next context ->
                    task {
                        let save = context.GetService<ProjectSave>()
                        let! project = context.BindJsonAsync<Projects.Project>()
                        let project = { project with Id = ShortGuid.fromGuid(Guid.NewGuid()) }
                        return! json (save project) next context
                    }

            GET >=> route "/projects"
                >=> authorize
                >=> send findAllProjects

            GET >=> routef "/projects/%s" (fun projectId ->
                authorize
                >=> json (findProjectById projectId))

            PUT >=> routef "/projects/%s" (fun projectId ->
                authorize
                >=> fun next context ->
                    task {
                        let! input = context.BindJsonAsync<UpdateGeneralRequest>()
                        let request = { input with Id = projectId }
                        return! json (updateGeneral request) next context
                    })

            DELETE >=> routef "/projects/%s" (fun id ->
                authorize
                >=> fun next context ->
                    let delete = context.GetService<ProjectDelete>()
                    json (delete id) next context)
        ]
