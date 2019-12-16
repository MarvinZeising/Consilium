namespace Consilium

module ProjectDatabase =

    open MongoDB.Driver
    open CommonTypes
    open ProjectTypes
    open Connection
    open Database

    let private collection = projectCollection;

    let private findProject<'a> = findOne projectCollection ProjectNotFound
    let private findProjects<'a> = find projectCollection

    let private updateById projectId updateBuilder =
        let filter = Builders<Project>.Filter.Eq((fun x -> x.Id), projectId)
        collection.UpdateOne(filter, updateBuilder) |> ignore

    let getProjects ctx =
        findProjects Builders<Project>.Filter.Empty ctx

    let getProjectById projectId ctx =
        let filter = Builders<Project>.Filter.Eq((fun x -> x.Id), projectId)
        findProject filter ctx

    let updateGeneral (request: UpdateGeneralRequest) =
        let updateBuilder =
            Builders<Project>.Update
                .Set((fun x -> x.Name), request.Name)
                .Set((fun x -> x.Email), request.Email)
        updateById request.Id updateBuilder

    let insertProject project =
        collection.InsertOne project
        project

    let deleteProject projectId =
        let update = Builders<Project>.Filter.Eq((fun x -> x.Id), projectId)
        collection.DeleteOne(update) |> ignore
