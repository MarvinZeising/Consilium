namespace Consilium

module ProjectController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2
    open ProjectTypes
    open ProjectDatabase
    open ProjectActions
    open Authentication
    open Controller

    let routes : HttpFunc -> HttpContext -> HttpFuncResult = authorize >=> choose [

        getRoute "/projects" (fun next ctx ->
            sendJson (getProjects ctx) next ctx)

        getRouteF "/projects/%s" (fun projectId next ctx ->
            sendJson (getProjectById projectId ctx) next ctx)

        postRoute "/projects" (fun next context ->
            task {
                let! project = context.BindJsonAsync<CreateProjectRequest>()
                return! sendJson (createProject project) next context
            })

        putRouteF "/projects/%s" (fun projectId next context ->
            task {
                let! input = context.BindJsonAsync<UpdateGeneralRequest>()
                let request = { input with Id = projectId }
                return! json (updateGeneral request) next context
            })

        deleteRouteF "/projects/%s" (fun id next ctx ->
            sendJson (deleteProject id) next ctx)
    ]
