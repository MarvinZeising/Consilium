namespace Consilium

module KnowledgeBaseDatabase =

    open MongoDB.Driver
    open CommonLibrary
    open KnowledgeBaseTypes
    open Connection
    open Database

    let result = new ResultBuilder()

    let private findTopics<'a> = find topicCollection

    let private builder = Builders<Topic>.Filter

    let private updateById topicId updateBuilder =
        let filter = builder.Eq((fun x -> x.Id), topicId)
        topicCollection.UpdateOne(filter, updateBuilder) |> ignore

    let getTopics ctx =
        findTopics builder.Empty ctx

    let updateTopic (request: UpdateTopicRequest) =
        updateById request.Id (Builders<Topic>.Update
            .Set((fun x -> x.Name), request.Name)
            .Set((fun x -> x.Order), request.Order))

    let insertTopic topic =
        topicCollection.InsertOne topic
        topic

    let deleteTopic topicId =
        result {
            return builder.Eq((fun x -> x.Id), topicId)
                   |> topicCollection.DeleteOne
        }
