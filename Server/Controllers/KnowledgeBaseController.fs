namespace Consilium

module KnowledgeBaseController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2
    open CommonTypes
    open KnowledgeBaseTypes
    open KnowledgeBaseActions
    open KnowledgeBaseDatabase
    open Authentication
    open Controller

    // TODO: check admin privileges
    let routes : HttpFunc -> HttpContext -> HttpFuncResult = choose [

        getRoute "/knowledge-base/topics" (fun next ctx ->
            sendJson (getTopics ctx) next ctx)

        POST >=> route "/knowledge-base/topics"
             >=> fun next context ->
                 task {
                     let! project = context.BindJsonAsync<CreateTopicRequest>()
                     return! sendJson (createTopic project) next context
                 }

        putRoute "/topics" (fun next context ->
            task {
                let! input = context.BindJsonAsync<UpdateTopicRequest>()
                return! json (updateTopic input) next context
            })

        DELETE >=> routef "/knowledge-base/topics/%s" (fun id ->
            authorize
            // TODO: check admin privileges
            >=> sendJson (deleteTopic id))

    ]
