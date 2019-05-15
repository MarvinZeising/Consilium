namespace Consilium

module UserDatabase =

    open MongoDB.Driver
    open UserTypes
    open Connection

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
 
    let updateLanguage (userId, language) =
        let updateBuilder = Builders<User>.Update.Set((fun x -> x.Language), language)
        updateById userId updateBuilder
 
    let updatePassword (userId, password) =
        let updateBuilder = Builders<User>.Update.Set((fun x -> x.Password), password)
        updateById userId updateBuilder
