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

let updateLanguage (collection : IMongoCollection<User>) (languageChange : LanguageChange) : unit option =
    let filter = Builders<User>.Filter.Eq((fun x -> x.Id), languageChange.Id)
    collection.Find(filter).ToEnumerable()
        |> Seq.tryLast
        |> Option.bind (fun _ ->
            let updateFilter = Builders<User>.Filter.Eq((fun x -> x.Id), languageChange.Id)
            let updateSetter = Builders<User>.Update.Set((fun x -> x.Language), languageChange.Language)
            collection.UpdateOne(updateFilter, updateSetter) |> Some)
        |> Option.bind (fun _ -> Some())

let updatePassword (collection : IMongoCollection<User>) (passwordChange : PasswordChange) : unit option =
    let filter = Builders<User>.Filter.Eq((fun x -> x.Id), passwordChange.Id)
    collection.Find(filter).ToEnumerable()
        |> Seq.tryLast
        |> Option.bind (isCorrectPassword passwordChange.Old)
        |> Option.bind (fun _ ->
            let hashedPassword = hash passwordChange.New
            let updateFilter = Builders<User>.Filter.Eq((fun x -> x.Id), passwordChange.Id)
            let updateSetter = Builders<User>.Update.Set((fun x -> x.Password), hashedPassword)
            collection.UpdateOne(updateFilter, updateSetter) |> Some)
        |> Option.bind (fun _ -> Some())

let delete (collection : IMongoCollection<User>) (id : string) : unit option =
    let update = Builders<User>.Filter.Eq((fun x -> x.Id), id)

    if collection.DeleteOne(update).DeletedCount > 0L
    then Some()
    else None

type IServiceCollection with
    member this.AddUserCollection (collection : IMongoCollection<User>) =
        this.AddSingleton<SignIn>(signIn collection) |> ignore
        this.AddSingleton<SignUp>(signUp collection) |> ignore
        this.AddSingleton<UserFind>(find collection) |> ignore
        this.AddSingleton<EmailAvailable>(emailAvailable collection) |> ignore
        this.AddSingleton<UpdateLanguage>(updateLanguage collection) |> ignore
        this.AddSingleton<UpdatePassword>(updatePassword collection) |> ignore
        this.AddSingleton<UserDelete>(delete collection) |> ignore
