namespace Consilium

module PersonDatabase =

    open MongoDB.Driver
    open CommonTypes
    open PersonTypes
    open Connection
    open Database

    let private collection = personCollection

    let private findPerson<'a> = findOne personCollection PersonNotFound
    let private findPersons<'a> = find personCollection

    let private builder = Builders<Person>.Filter

    let private updateByIds userId personId updateBuilder =
        let filter = builder.And [|
            builder.Eq((fun x -> x.UserId), userId)
            builder.Eq((fun x -> x.Id), personId)
        |]
        collection.UpdateOne(filter, updateBuilder)

    let getPersons userId =
        builder.Eq((fun x -> x.UserId), userId)
        |> findPersons

    let updateGeneral (request: UpdateGeneralRequest) =
        updateByIds request.UserId request.Id (Builders<Person>.Update
            .Set((fun x -> x.Firstname), request.Firstname)
            .Set((fun x -> x.Lastname), request.Lastname))

    let insertPerson person =
        collection.InsertOne person
        person

    let deletePerson personId =
        let update = builder.Eq((fun x -> x.Id), personId)
        collection.DeleteOne(update)
