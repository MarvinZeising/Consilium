namespace Consilium

module KnowledgeBaseActions =

    open Giraffe
    open System
    open CommonValidation
    open Actions
    open KnowledgeBaseTypes
    open KnowledgeBaseRepository

    let updateTopic (input: UpdateTopicRequest) =
        result {
            let! validatedName = input.Name |> validateName

            return! updateTopic { input with Name = validatedName }
        }

    let createTopic (input: CreateTopicRequest) =
        result {
            let! validatedName = input.Name |> validateName

            return! insertTopic { Id = ShortGuid.fromGuid(Guid.NewGuid())
                                  ProjectId = input.ProjectId
                                  Name = validatedName
                                  Order = input.Order }
        }

    let deleteTopic topicId =
        result {
            return! deleteTopic topicId
        }
