namespace Consilium

module ProjectParticipationDatabase =

    open MongoDB.Driver
    open CommonTypes
    open ProjectParticipationTypes
    open Connection
    open Database

    let private collection = projectParticipationCollection

    let private findPerson<'a> = findOne collection ProjectParticipationNotFound
    let private findProjectParticipations<'a> = find collection

    let private builder = Builders<ProjectParticipation>.Filter

    let private updateById projectParticipationId updateBuilder =
        let filter = builder.Eq((fun x -> x.Id), projectParticipationId)
        collection.UpdateOne(filter, updateBuilder)

    let getProjectParticipations personId =
        builder.Eq((fun x -> x.PersonId), personId)
        |> findProjectParticipations

    let updateProjectParticipation (request: UpdateProjectParticipationRequest) =
        Builders<ProjectParticipation>.Update
            .Set((fun x -> x.Status), request.Status)
        |> updateById request.Id

    let insertProjectParticipation projectParticipation =
        collection.InsertOne projectParticipation
        projectParticipation

    let deleteProjectParticipation projectParticipationId =
        builder.Eq((fun x -> x.Id), projectParticipationId)
        |> collection.DeleteOne
