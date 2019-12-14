namespace Consilium

module KnowledgeBaseDatabase =

    open MongoDB.Driver
    open KnowledgeBaseTypes
    open Connection

    let private find (filter : FilterDefinition<Topic>) =
        topicsCollection.Find(filter).ToEnumerable()

    let private updateById topicId updateBuilder =
        let filter = Builders<Topic>.Filter.Eq((fun x -> x.Id), topicId)
        topicsCollection.UpdateOne(filter, updateBuilder) |> ignore

    let getAllTopics _ =
        Builders<Topic>.Filter.Empty |> find |> Seq.toArray

    let getTopicsByProjectId (projectId: string) =
        let filter = Builders<Topic>.Filter.Eq((fun x -> x.ProjectId), projectId)
        filter |> find |> Seq.toArray

    let updateTopic (request: UpdateTopicRequest) =
        let updateBuilder = Builders<Topic>.Update.Set((fun x -> x.Name), request.Name)
        updateById request.Id updateBuilder

    let insertTopic topic =
        topicsCollection.InsertOne topic
        topic

    let deleteTopic topicId =
        let filter = Builders<Topic>.Filter.Eq((fun x -> x.Id), topicId)
        topicsCollection.DeleteOne(filter) |> ignore
