namespace Consilium

module UserRepository =

    open CommonLibrary
    open CommonTypes
    open UserDatabase

    let getUserById userId =
        try
            let user = getUserById userId
            match user with
            | Some user -> succeed user
            | None -> fail [UserNotFound]
        with
        | ex -> fail [ServerException ex]

    let updateEmail<'a> =
        tryCatch updateEmail (fun ex -> [ServerException ex])

    let updateLanguage<'a> =
        tryCatch updateLanguage (fun ex -> [ServerException ex])

    let updatePassword<'a> =
        tryCatch updatePassword (fun ex -> [ServerException ex])
