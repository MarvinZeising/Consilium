namespace Consilium

module KnowledgeBaseDatabase =

    open MongoDB.Driver
    open KnowledgeBaseTypes
    open Connection
    open Database

    let private findTopics<'a> = find topicCollection

    let private updateById topicId updateBuilder =
        printfn "UPDATE: topic: %s" topicId
        let filter = Builders<Topic>.Filter.Eq((fun x -> x.Id), topicId)
        topicCollection.UpdateOne(filter, updateBuilder) |> ignore

    let getTopics ctx =
        findTopics Builders<Topic>.Filter.Empty ctx

    let updateTopic (request: UpdateTopicRequest) =
        let updateBuilder =
            Builders<Topic>.Update
                .Set((fun x -> x.Name), request.Name)
                .Set((fun x -> x.Order), request.Order)
        updateById request.Id updateBuilder

    let insertTopic topic =
        printfn "INSERT: topic"
        topicCollection.InsertOne topic
        topic

    let deleteTopic topicId =
        printfn "DELETE: topic: %s" topicId
        let filter = Builders<Topic>.Filter.Eq((fun x -> x.Id), topicId)
        topicCollection.DeleteOne(filter) |> ignore
