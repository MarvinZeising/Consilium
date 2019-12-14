namespace Consilium

module Repository =

    open CommonLibrary
    open CommonTypes

    let tryCatchError f =
        tryCatch f (fun ex -> [ServerException ex])
