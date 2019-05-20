namespace Consilium

module Connection =

    open MongoDB.Driver
    open UserTypes
    open ProjectTypes

    let private mongo = MongoClient ("mongodb://localhost:27017/")
    let private db = mongo.GetDatabase "ConsiliumDb"

    let userCollection = db.GetCollection<User>("users")
    let projectCollection = db.GetCollection<Project>("projects")
