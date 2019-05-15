namespace Consilium

module UserRepository =

    open CommonLibrary
    open CommonTypes
    open UserDatabase

    let throwServerException ex =
        [ServerException ex]

    let getUserById userId =
        try
            let user = getUserById userId
            match user with
            | Some user -> succeed user
            | None -> fail [UserNotFound]
        with
        | ex -> fail [ServerException ex]

    let getUserByEmail email =
        try
            let user = getUserByEmail email
            match user with
            | Some user -> succeed user
            | None -> fail [UserNotFound]
        with
        | ex -> fail [ServerException ex]

    let updateEmail<'a> =
        tryCatch updateEmail throwServerException

    let updateLanguage<'a> =
        tryCatch updateLanguage throwServerException

    let updatePassword<'a> =
        tryCatch updatePassword throwServerException

    let deleteUser<'a> =
        tryCatch deleteUser throwServerException
