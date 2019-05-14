namespace Consilium

module UserDatabase =

    open MongoDB.Driver
    open CommonLibrary
    open DomainTypes

    let updateDatabase (collection : IMongoCollection<User>) (userId, email) =
        let filter = Builders<User>.Filter.Eq((fun x -> x.Id), userId)
        let update = Builders<User>.Update.Set((fun x -> x.Email), email)
        collection.UpdateOne(filter, update) |> ignore
