namespace Consilium

module ProjectDatabase =

    open MongoDB.Driver
    open ProjectTypes
    open Connection

    let private collection = projectCollection;

    let private find (filter : FilterDefinition<Project>) =
        collection.Find(filter).ToEnumerable()

    let private updateById projectId updateBuilder =
        let filter = Builders<Project>.Filter.Eq((fun x -> x.Id), projectId)
        collection.UpdateOne(filter, updateBuilder) |> ignore

    let getAllProjects _ =
        Builders.Filter.Empty |> find |> Seq.toArray

    let getProjectById projectId =
        let filter = Builders<Project>.Filter.Eq((fun x -> x.Id), projectId)
        filter |> find |> Seq.tryLast

    let updateGeneral (request: UpdateGeneralRequest) =
        let updateBuilder =
            Builders<Project>.Update
                .Set((fun x -> x.Name), request.Name)
                .Set((fun x -> x.Email), request.Email)
        updateById request.Id updateBuilder

    //let insertProject project =
    //    collection.InsertOne project

    //let deleteProject projectId =
    //    let update = Builders<Project>.Filter.Eq((fun x -> x.Id), projectId)
    //    collection.DeleteOne(update)
