namespace Consilium

module KnowledgeBaseController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2
    open CommonTypes
    open KnowledgeBaseTypes
    open KnowledgeBaseActions
    open Authentication
    open Controller

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [

            GET >=> route "/knowledge-base/topics"
                >=> authorize
                >=> sendJson findAllTopics

            GET >=> routef "/knowledge-base/topics/by-project/%s" (fun projectId ->
                authorize
                // TODO: check admin privileges
                >=> sendJson (findTopicsByProjectId projectId))

            POST >=> route "/knowledge-base/topics"
                >=> authorize // TODO: check admin privileges
                >=> fun next context ->
                    task {
                        let! project = context.BindJsonAsync<CreateTopicRequest>()
                        return! sendJson (createTopic project) next context
                    }

            PUT >=> route "/knowledge-base/topics"
                >=> authorize
                // TODO: check admin privileges
                >=> fun next context ->
                    task {
                        let! input = context.BindJsonAsync<UpdateTopicRequest>()
                        return! json (updateTopic input) next context
                    }

            DELETE >=> routef "/knowledge-base/topics/%s" (fun id ->
                authorize
                // TODO: check admin privileges
                >=> sendJson (deleteTopic id))

        ]
