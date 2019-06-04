namespace Consilium

module ProjectController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2
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
                        let! project = context.BindJsonAsync<CreateProjectRequest>()
                        return! sendJson (createProject project) next context
                    }

            GET >=> route "/projects"
                >=> authorize
                >=> sendJson findAllProjects

            GET >=> routef "/projects/%s" (fun projectId ->
                authorize
                >=> sendJson (findProjectById projectId))

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
                >=> sendJson (deleteProject id))
        ]
