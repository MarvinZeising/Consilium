module Users.UserCollection

open Users
open MongoDB.Driver
open Microsoft.Extensions.DependencyInjection
open Giraffe
open System
open Authentication

let find (collection : IMongoCollection<User>) (email : string) : User option =
    let filter = Builders.Filter.Eq((fun (x : User) -> x.Email), email)
    collection.Find(filter).ToEnumerable() |> Seq.tryLast

let emailAvailable (collection : IMongoCollection<User>) (email : string) : bool =
     find collection email |> Option.isNone

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
    let generateTokenIfPasswordIsCorrect credentials (user : User) : string option =
        if verify credentials.Password user.Password
        then credentials.Email |> generateToken user.Id |> Some
        else None

    let filter = Builders<User>.Filter.Eq((fun x -> x.Email), credentials.Email)
    let user = collection.Find(filter).ToEnumerable() |> Seq.tryLast
    user |> Option.bind (generateTokenIfPasswordIsCorrect credentials)

let delete (collection : IMongoCollection<User>) (id : string) : unit option =
    if collection.DeleteOne(Builders.Filter.Eq((fun x -> x.Id), id)).DeletedCount > 0L
    then Some()
    else None

type IServiceCollection with
    member this.AddUserCollection (collection : IMongoCollection<User>) =
        this.AddSingleton<UserFind>(find collection) |> ignore
        this.AddSingleton<UserDelete>(delete collection) |> ignore
        this.AddSingleton<SignIn>(signIn collection) |> ignore
        this.AddSingleton<SignUp>(signUp collection) |> ignore
        this.AddSingleton<EmailAvailable>(emailAvailable collection) |> ignore
