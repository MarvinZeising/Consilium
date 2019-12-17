namespace Consilium

module Controller =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open Microsoft.Extensions.Logging
    open FSharp.Control.Tasks.V2
    open CommonTypes

    let private logErrors errors (ctx: HttpContext) =
        let logger = ctx.GetLogger("Consilium.Controller.logError")
        logger.LogError (Printf.sprintf "%A" errors)

    let private returnError errors =
        errors |> List.head |> mapErrorCode |> setStatusCode
        >=> json (List.map string errors)

    let sendJson (result: Result<'a,Error list>) (next: HttpFunc) (ctx: HttpContext) =
        match result with
        | Ok value -> json value next ctx
        | Error errors ->
            logErrors errors ctx |> ignore
            returnError errors next ctx

    let sendString (result: Result<string,Error list>) next ctx =
        match result with
        | Ok value ->
            printfn "%s" value
            htmlString value next ctx
        | Error errors ->
            logErrors errors ctx |> ignore
            returnError errors next ctx

    let private logRoute prefix path (ctx: HttpContext) =
        let logger = ctx.GetLogger("Consilium.Controller.Route")
        let date = System.DateTime.UtcNow.ToLongTimeString()
        logger.LogInformation (Printf.sprintf "%s ROUTE %s: %s" date prefix path)

    let private routeWithLogging prefix path (handle: HttpHandler) =
        route path >=> (fun next ctx ->
            logRoute prefix path ctx
            handle next ctx)

    let private routeFWithLogging prefix path handle =
        routef path (fun value next ctx ->
            logRoute prefix (path.ToString()) ctx
            handle value next ctx)

    let handlePayload = fun func request (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let result = func request ctx
            match result with
            | Ok value -> return! Successful.OK value next ctx
            | Error errors -> return! returnError errors next ctx
        }

    let handleRequest = fun func (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let result = func ctx
            match result with
            | Ok value -> return! Successful.OK value next ctx
            | Error errors -> return! returnError errors next ctx
        }

    let getRoute path handle =
        GET >=> routeWithLogging "GET" path handle

    let getRouteF path handle =
        GET >=> routeFWithLogging "GET" path handle

    let postRoute path handle =
        printfn "init %s" path
        POST >=> routeWithLogging "POST" path handle

    let postRouteF path handle =
        POST >=> routeFWithLogging "POST" path handle

    let putRoute path handle =
        PUT >=> routeWithLogging "PUT" path handle

    let putRouteF path handle =
        PUT >=> routeFWithLogging "PUT" path handle

    let deleteRoute path handle =
        DELETE >=> routeWithLogging "DELETE" path handle

    let deleteRouteF path handle =
        DELETE >=> routeFWithLogging "DELETE" path handle

    let routes : HttpFunc -> HttpContext -> HttpFuncResult = choose [
        getRoute "/status" (sendString (Ok "Server is up and running"))
    ]