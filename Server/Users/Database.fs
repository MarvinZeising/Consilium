module Users.UserCollection

open Users
open MongoDB.Driver
open Microsoft.Extensions.DependencyInjection
open System.Linq

let find (collection : IMongoCollection<User>) (criteria : UserCriteria) : User[] =
    let filter =
        match criteria with
        | All ->
            Builders.Filter.Empty
        | Username username ->
            Builders.Filter.Eq((fun (x : User) -> x.Email), username) // TODO: fix

    collection.Find(filter).ToEnumerable() |> Seq.toArray

let emailAvailable (collection : IMongoCollection<User>) (email : string) : bool =
    let filter = Builders.Filter.Eq((fun (x : User) -> x.Email), email)
    collection.Find(filter).ToEnumerable().ToArray() |> Array.isEmpty

let save (collection : IMongoCollection<User>) (user : User) : User =
    let users = collection.Find(fun x -> x.Id = user.Id).ToEnumerable()

    match Seq.isEmpty users with
    | true -> collection.InsertOne user
    | false ->
        let filter = Builders<User>.Filter.Eq((fun x -> x.Id), user.Id)
        let update =
            Builders<User>.Update
                .Set((fun x -> x.Email), user.Email)
                .Set((fun x -> x.Password), user.Password)

        collection.UpdateOne(filter, update) |> ignore

    user

let authenticate (collection : IMongoCollection<User>) (credentials : Credentials) : User option =
    let usernameFilter = Builders<User>.Filter.Eq((fun x -> x.Email), credentials.Email)
    let passwordFilter = Builders<User>.Filter.Eq((fun x -> x.Password), credentials.Password)
    let filter = Builders.Filter.And [| usernameFilter; passwordFilter |]

    collection.Find(filter).ToEnumerable() |> Seq.tryLast

type IServiceCollection with
    member this.AddUserCollection (collection : IMongoCollection<User>) =
        this.AddSingleton<UserFind>(find collection) |> ignore
        this.AddSingleton<EmailAvailable>(emailAvailable collection) |> ignore
        this.AddSingleton<UserSave>(save collection) |> ignore
        this.AddSingleton<Authenticate>(authenticate collection) |> ignore
