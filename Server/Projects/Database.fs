module Projects.ProjectCollection

open Projects
open MongoDB.Driver
open Microsoft.Extensions.DependencyInjection

let find (collection : IMongoCollection<Project>) (criteria : ProjectCriteria) : Project[] =
    match criteria with
    | All -> collection.Find(Builders.Filter.Empty).ToEnumerable() |> Seq.toArray

let findOne (collection : IMongoCollection<Project>) (id : string) : Project option =
    let filter = Builders<Project>.Filter.Eq((fun x -> x.Id), id)
    collection.Find(filter).ToEnumerable() |> Seq.tryLast

let save (collection : IMongoCollection<Project>) (project : Project) : Project =
    let projects = collection.Find(fun x -> x.Id = project.Id).ToEnumerable()

    match Seq.isEmpty projects with
    | true -> collection.InsertOne project
    | false ->
        let filter = Builders.Filter.Eq((fun x -> x.Id), project.Id)
        let update =
            Builders.Update
                .Set((fun x -> x.Name), project.Name)
                .Set((fun x -> x.Email), project.Email)

        collection.UpdateOne(filter, update) |> ignore

    project

let delete (collection : IMongoCollection<Project>) (id : string) : bool =
    collection.DeleteOne(Builders.Filter.Eq((fun x -> x.Id), id)).DeletedCount > 0L

type IServiceCollection with
    member this.AddProjectCollection (collection : IMongoCollection<Project>) =
        this.AddSingleton<ProjectFind>(find collection) |> ignore
        this.AddSingleton<ProjectFindById>(findOne collection) |> ignore
        this.AddSingleton<ProjectSave>(save collection) |> ignore
        this.AddSingleton<ProjectDelete>(delete collection) |> ignore
