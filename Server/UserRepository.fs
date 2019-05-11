namespace Consilium

/// ===========================================
/// Database functions
/// ===========================================
module UserRepository =

    open CommonLibrary
    open DomainTypes

    let updateDatabase input =
       ()   // dummy dead-end function for now

    // new function to handle exceptions
    let updateDatabaseStep (request : Request) =
        tryCatch (tee updateDatabase) (fun ex -> ex.Message) request

