namespace Consilium

module Database =

    open Microsoft.AspNetCore.Http
    open Microsoft.Extensions.Logging
    open MongoDB.Driver
    open Giraffe
    open CommonLibrary
    open CommonTypes

    let tryCatchError f =
        tryCatch f (fun ex -> [ServerException ex])

    let findOne (collection: IMongoCollection<'T>) (notFoundError: Error) (filter : FilterDefinition<'T>) (ctx: HttpContext) =
        let logger = ctx.GetLogger("Consilium.Database.findOne")
        let collectionName = collection.CollectionNamespace.CollectionName
        logger.LogInformation (Printf.sprintf "Querying %s with filter: %A" collectionName filter)

        try
            let value = collection.Find(filter).ToList() |> Seq.tryLast
            match value with
            | Some value -> Ok value
            | None -> Error [notFoundError]
        with
        | ex -> Error [ServerException ex]

    let find (collection: IMongoCollection<'T>) (filter : FilterDefinition<'T>) (ctx: HttpContext) =
        let logger = ctx.GetLogger("Consilium.Database.find")
        let collectionName = collection.CollectionNamespace.CollectionName
        logger.LogInformation (Printf.sprintf "Querying %s with filter: %A" collectionName filter)

        try
            collection.Find(filter).ToList() |> Ok
        with
        | ex -> Error [ServerException ex]
