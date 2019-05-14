namespace Consilium

open MongoDB.Driver

/// ===========================================
/// Database functions
/// ===========================================
module UserRepository =

    open CommonLibrary
    open DomainTypes
    open UserDatabase

    let private mongo = MongoClient ("mongodb://localhost:27017/")
    let private db = mongo.GetDatabase "ConsiliumDb"
    let private collection = db.GetCollection<User>("users")

    // new function to handle exceptions
    let updateDatabase<'a> =
        tryCatch (updateDatabase collection) (fun ex -> [ServerException ex])
