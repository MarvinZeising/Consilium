namespace Consilium

module Connection =

    open System
    open MongoDB.Driver
    open UserTypes
    open PersonTypes
    open ProjectTypes

    type Config = {
        DbUrl: string
        DbName: string
        TestServerUrl: string
    }

    let getEnvVar name defaultValue =
        match Environment.GetEnvironmentVariable name with
        | null -> defaultValue
        | value -> value

    let config =
        { DbUrl = getEnvVar "DB_URL" "mongodb://localhost:27017/"
          DbName = getEnvVar "DB_NAME" "ConsiliumDb"
          TestServerUrl = getEnvVar "SERVER_URL" "http://localhost:5000" }

    let private mongo = MongoClient config.DbUrl
    let private db = mongo.GetDatabase config.DbName

    let userCollection = db.GetCollection<User> "users"
    let personCollection = db.GetCollection<Person> "persons"
    let projectCollection = db.GetCollection<Project> "projects"
