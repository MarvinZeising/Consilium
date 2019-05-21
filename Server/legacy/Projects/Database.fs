module Projects.ProjectCollection

open Projects
open MongoDB.Driver
open Microsoft.Extensions.DependencyInjection

let save (collection : IMongoCollection<Project>) (project : Project) : Project =
    let projects = collection.Find(fun x -> x.Id = project.Id).ToEnumerable() |> List.ofSeq

    match projects with
    | [] -> collection.InsertOne project
    | _ ->
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
        this.AddSingleton<ProjectSave>(save collection) |> ignore
        this.AddSingleton<ProjectDelete>(delete collection) |> ignore
