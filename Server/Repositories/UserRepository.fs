namespace Consilium

module UserRepository =

    open CommonLibrary
    open CommonTypes
    open UserDatabase

    let tryCatchError f =
        tryCatch f (fun ex -> [ServerException ex])

    let getUserById userId =
        try
            let user = getUserById userId
            match user with
            | Some user -> Ok user
            | None -> Error [UserNotFound]
        with
        | ex -> Error [ServerException ex]

    let getUserByEmail email =
        try
            let user = getUserByEmail email
            match user with
            | Some user -> Ok user
            | None -> Error [UserNotFound]
        with
        | ex -> Error [ServerException ex]

    let updateEmail<'a> =
        updateEmail |> tryCatchError

    let updateLanguage<'a> =
        updateLanguage |> tryCatchError

    let updatePassword<'a> =
        updatePassword |> tryCatchError

    let insertUser<'a> =
        insertUser |> tryCatchError

    let deleteUser<'a> =
        deleteUser |> tryCatchError
