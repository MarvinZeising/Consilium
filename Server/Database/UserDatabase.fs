namespace Consilium

module UserDatabase =

    open MongoDB.Driver
    open UserTypes
    open Connection

    let updateById userId updateBuilder =
        let filter = Builders<User>.Filter.Eq((fun x -> x.Id), userId)
        collection.UpdateOne(filter, updateBuilder) |> ignore

    let updateEmail (userId, email) =
        let updateBuilder = Builders<User>.Update.Set((fun x -> x.Email), email)
        updateById userId updateBuilder
 
    let updateLanguage (userId, language) =
        let updateBuilder = Builders<User>.Update.Set((fun x -> x.Language), language)
        updateById userId updateBuilder
