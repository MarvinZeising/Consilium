namespace Consilium

module Connection =

    open MongoDB.Driver
    open UserTypes
    open PersonTypes
    open ProjectTypes

    let private mongo = MongoClient ("mongodb://db:27017/")
    let private db = mongo.GetDatabase "ConsiliumDb"

    let userCollection = db.GetCollection<User>("users")
    let personCollection = db.GetCollection<Person>("persons")
    let projectCollection = db.GetCollection<Project>("projects")
