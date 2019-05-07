module Users.UserCollection

open Users
open MongoDB.Driver
open Microsoft.Extensions.DependencyInjection
open System.Linq
open Giraffe
open System

let find (collection : IMongoCollection<User>) (_ : string) : User[] =
    collection.Find(Builders.Filter.Empty).ToEnumerable() |> Seq.toArray

let emailAvailable (collection : IMongoCollection<User>) (email : string) : bool =
    let filter = Builders.Filter.Eq((fun (x : User) -> x.Email), email)
    collection.Find(filter).ToEnumerable().ToArray() |> Array.isEmpty

let signUp (collection : IMongoCollection<User>) (credentials : Credentials) : bool =
    let users = collection.Find(fun x -> x.Email = credentials.Email).ToEnumerable()
    let emailNotAlreadyExisting = users |> Seq.isEmpty

    if emailNotAlreadyExisting then
        let user = {
            Id = ShortGuid.fromGuid(Guid.NewGuid())
            Email = credentials.Email
            Password = credentials.Password
        }
        collection.InsertOne user

    emailNotAlreadyExisting

let signIn (collection : IMongoCollection<User>) (credentials : Credentials) : bool =
    let emailFilter = Builders<User>.Filter.Eq((fun x -> x.Email), credentials.Email)
    let passwordFilter = Builders<User>.Filter.Eq((fun x -> x.Password), credentials.Password)
    let filter = Builders.Filter.And [| emailFilter; passwordFilter |]

    collection.Find(filter).ToEnumerable().ToArray() |> Array.isEmpty |> not
    // TODO: return JWT token

type IServiceCollection with
    member this.AddUserCollection (collection : IMongoCollection<User>) =
        this.AddSingleton<UserFind>(find collection) |> ignore
        this.AddSingleton<SignIn>(signIn collection) |> ignore
        this.AddSingleton<SignUp>(signUp collection) |> ignore
        this.AddSingleton<EmailAvailable>(emailAvailable collection) |> ignore
