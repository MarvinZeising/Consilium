namespace Consilium

module UserRepository =

    open CommonLibrary
    open CommonTypes
    open UserDatabase

    let updateEmail<'a> =
        tryCatch updateEmail (fun ex -> [ServerException ex])

    let updateLanguage<'a> =
        tryCatch updateLanguage (fun ex -> [ServerException ex])
