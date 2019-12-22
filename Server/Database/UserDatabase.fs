namespace Consilium

module UserDatabase =

    open MongoDB.Driver
    open UserTypes
    open Connection

    let collection = userCollection

    let private find (filter : FilterDefinition<User>) =
        collection.Find(filter).ToEnumerable()

    let private updateById userId updateBuilder =
        let filter = Builders<User>.Filter.Eq((fun x -> x.Id), userId)
        collection.UpdateOne(filter, updateBuilder) |> ignore

    let getUserById userId =
        let filter = Builders<User>.Filter.Eq((fun x -> x.Id), userId)
        filter |> find |> Seq.tryLast

    let getUserByEmail email =
        let filter = Builders<User>.Filter.Eq((fun x -> x.Email), email)
        filter |> find |> Seq.tryLast

    let updateEmail (userId, email) =
        let updateBuilder = Builders<User>.Update.Set((fun x -> x.Email), email)
        updateById userId updateBuilder
 
    let updatePassword (userId, password) =
        let updateBuilder = Builders<User>.Update.Set((fun x -> x.Password), password)
        updateById userId updateBuilder

    // new pattern
    let updateInterface (request: UpdateInterfaceRequest) =
        updateById request.Id (Builders<User>.Update
            .Set((fun x -> x.Language), request.Language)
            .Set((fun x -> x.Theme), request.Theme))

    let insertUser user =
        collection.InsertOne user

    let deleteUser userId =
        let update = Builders<User>.Filter.Eq((fun x -> x.Id), userId)
        collection.DeleteOne(update)
