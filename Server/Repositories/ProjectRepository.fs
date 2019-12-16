namespace Consilium

module ProjectRepository =

    open ProjectDatabase
    open Repository

    let updateGeneral<'a> =
        updateGeneral |> tryCatchError

    let insertProject<'a> =
        insertProject |> tryCatchError

    let deleteProject<'a> =
        deleteProject |> tryCatchError
