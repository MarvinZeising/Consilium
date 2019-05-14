namespace Consilium

module Connection =

    open MongoDB.Driver
    open UserTypes

    let private mongo = MongoClient ("mongodb://localhost:27017/")
    let private db = mongo.GetDatabase "ConsiliumDb"

    let collection = db.GetCollection<User>("users")
