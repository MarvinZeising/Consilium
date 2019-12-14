namespace Consilium

module ProjectRepository =

    open CommonLibrary
    open CommonTypes
    open ProjectDatabase
    open Repository

    let getAllProjects<'a> =
        getAllProjects |> tryCatchError

    let getProjectById projectId =
        try
            let project = getProjectById projectId
            match project with
            | Some project -> Ok project
            | None -> Error [ProjectNotFound]
        with
        | ex -> Error [ServerException ex]

    let updateGeneral<'a> =
        updateGeneral |> tryCatchError

    let insertProject<'a> =
        insertProject |> tryCatchError

    let deleteProject<'a> =
        deleteProject |> tryCatchError
