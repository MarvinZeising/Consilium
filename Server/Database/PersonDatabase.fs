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

    let private updateById personId updateBuilder =
        let filter = Builders<Person>.Filter.Eq((fun x -> x.Id), personId)
        collection.UpdateOne(filter, updateBuilder) |> ignore

    let getPersons userId =
        let filter = Builders<Person>.Filter.Eq((fun x -> x.UserId), userId)
        filter |> findPersons

    let updateGeneral (request: UpdateNameRequest) =
        let updateBuilder =
            Builders<Person>.Update
                .Set((fun x -> x.Firstname), request.Firstname)
                .Set((fun x -> x.Lastname), request.Lastname)
        updateById request.PersonId updateBuilder

    let insertPerson person =
        collection.InsertOne person
        person

    let deletePerson personId =
        let update = Builders<Person>.Filter.Eq((fun x -> x.Id), personId)
        collection.DeleteOne(update)
