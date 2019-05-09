module Users.UserCollection

open Users
open MongoDB.Driver
open Microsoft.Extensions.DependencyInjection
open Giraffe
open System
open Authentication

let find (collection : IMongoCollection<User>) (_ : string) : User option =
    collection.Find(Builders.Filter.Empty).ToEnumerable() |> Seq.tryLast

let emailAvailable (collection : IMongoCollection<User>) (email : string) : bool =
    let filter = Builders.Filter.Eq((fun (x : User) -> x.Email), email)
    collection.Find(filter).ToEnumerable() |> Seq.isEmpty

let signUp (collection : IMongoCollection<User>) (credentials : Credentials) : bool =
    let users = collection.Find(fun x -> x.Email = credentials.Email).ToEnumerable()
    let emailNotAlreadyExisting = users |> Seq.isEmpty

    if emailNotAlreadyExisting then
        let hashedPassword = hash credentials.Password
        let user = {
            Id = ShortGuid.fromGuid(Guid.NewGuid())
            Email = credentials.Email
            Password = hashedPassword
        }
        collection.InsertOne user

    emailNotAlreadyExisting

let signIn (collection : IMongoCollection<User>) (credentials : Credentials) : string option =
    let filter = Builders<User>.Filter.Eq((fun x -> x.Email), credentials.Email)
    let user = collection.Find(filter).ToEnumerable() |> Seq.tryLast
    match user with
    | Some user ->
        if verify credentials.Password user.Password then
            credentials.Email |> generateToken |> Some
        else
            None
    | None ->
        None

type IServiceCollection with
    member this.AddUserCollection (collection : IMongoCollection<User>) =
        this.AddSingleton<UserFind>(find collection) |> ignore
        this.AddSingleton<SignIn>(signIn collection) |> ignore
        this.AddSingleton<SignUp>(signUp collection) |> ignore
        this.AddSingleton<EmailAvailable>(emailAvailable collection) |> ignore
