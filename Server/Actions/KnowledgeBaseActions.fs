namespace Consilium

module KnowledgeBaseActions =

    open Giraffe
    open System
    open CommonValidation
    open Actions
    open KnowledgeBaseTypes
    open KnowledgeBaseRepository

    let findAllTopics =
        getAllTopics ()

    let findTopicsByProjectId =
        getTopicsByProjectId

    let updateTopic (input: UpdateTopicRequest) =
        result {
            let! validatedName = input.Name |> validateName

            return! updateTopic { input with Name = validatedName }
        }

    let createTopic (input: CreateTopicRequest) =
        result {
            let topicId = ShortGuid.fromGuid(Guid.NewGuid())
            let! validatedName = input.Name |> validateName

            return! insertTopic { Id = topicId
                                  ProjectId = input.ProjectId
                                  Name = validatedName }
        }

    let deleteTopic topicId =
        result {
            return! deleteTopic topicId
        }
