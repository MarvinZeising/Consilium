module Users.UserCollection

open Users
open MongoDB.Driver
open Microsoft.Extensions.DependencyInjection
open Giraffe
open System
open Consilium
open Authentication

let private isCorrectPassword password (user : User) : User option =
    if verify password user.Password
    then Some user
    else None

let signUp (collection : IMongoCollection<User>) (credentials : Credentials) : bool =
    let users = collection.Find(fun x -> x.Email = credentials.Email).ToEnumerable()
    let emailNotAlreadyExisting = users |> Seq.isEmpty

    if emailNotAlreadyExisting then
        let hashedPassword = hash credentials.Password
        let user = {
            Id = ShortGuid.fromGuid(Guid.NewGuid())
            Email = credentials.Email
            Password = hashedPassword
            Language = "en-US"
        }
        collection.InsertOne user

    emailNotAlreadyExisting

let signIn (collection : IMongoCollection<User>) (credentials : Credentials) : string option =
    let filter = Builders<User>.Filter.Eq((fun x -> x.Email), credentials.Email)
    collection.Find(filter).ToEnumerable()
        |> Seq.tryLast
        |> Option.bind (isCorrectPassword credentials.Password)
        |> Option.bind (fun user -> generateToken user.Id user.Email |> Some)

type IServiceCollection with
    member this.AddUserCollection (collection : IMongoCollection<User>) =
        this.AddSingleton<SignIn>(signIn collection) |> ignore
        this.AddSingleton<SignUp>(signUp collection) |> ignore
