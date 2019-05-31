namespace Consilium

module PersonDatabase =

    open MongoDB.Driver
    open PersonTypes
    open Connection

    let private collection = personCollection;

    let private find (filter : FilterDefinition<Person>) =
        collection.Find(filter).ToEnumerable()

    let private updateById personId updateBuilder =
        let filter = Builders<Person>.Filter.Eq((fun x -> x.Id), personId)
        collection.UpdateOne(filter, updateBuilder) |> ignore

    let getAllPersons _ =
        Builders.Filter.Empty |> find |> Seq.toArray

    let getPersonById personId =
        let filter = Builders<Person>.Filter.Eq((fun x -> x.Id), personId)
        filter |> find |> Seq.tryLast

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
        collection.DeleteOne(update) |> ignore
