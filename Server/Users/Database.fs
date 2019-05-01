module Users.UserCollection

open Users
open MongoDB.Driver
open Microsoft.Extensions.DependencyInjection

let find (collection : IMongoCollection<User>) (criteria : UserCriteria) : User[] =
    match criteria with
    | UserCriteria.All -> collection.Find(Builders.Filter.Empty).ToEnumerable() |> Seq.toArray

let save (collection : IMongoCollection<User>) (user : User) : User =
    let users = collection.Find(fun x -> x.Id = user.Id).ToEnumerable()

    match Seq.isEmpty users with
    | true -> collection.InsertOne user
    | false ->
        let filter = Builders<User>.Filter.Eq((fun x -> x.Id), user.Id)
        let update =
            Builders<User>.Update
                .Set((fun x -> x.Username), user.Username)
                .Set((fun x -> x.Password), user.Password)

        collection.UpdateOne(filter, update) |> ignore

    user

type IServiceCollection with
    member this.AddUserCollection (collection : IMongoCollection<User>) =
        this.AddSingleton<UserFind>(find collection) |> ignore
        this.AddSingleton<UserSave>(save collection) |> ignore
