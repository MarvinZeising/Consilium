namespace Consilium

module Controller =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open CommonTypes

    let private returnError errors =
        errors
        |> List.head
        |> mapErrorCode
        |> setStatusCode
        >=> json (List.map string errors)

    let sendJson result =
        result |> CommonLibrary.either json returnError

    let sendString (result : Result<string,Error list>) =
        result |> CommonLibrary.either htmlString returnError

    let private routeWithLogging prefix path func =
        printfn "ROUTE %s: %s" prefix path
        route path >=> func

    let private routeFWithLogging prefix path func =
        printfn "ROUTE %s: %A" prefix path
        routef path func

    let getRoute path func =
        GET >=> routeWithLogging "GET" path func

    let getRouteF path func =
        GET >=> routeFWithLogging "GET" path func

    let postRoute path func =
        POST >=> routeWithLogging "POST" path func

    let postRouteF path func =
        POST >=> routeFWithLogging "POST" path func

    let putRoute path func =
        PUT >=> routeWithLogging "PUT" path func

    let putRouteF path func =
        PUT >=> routeFWithLogging "PUT" path func

    let deleteRoute path func =
        DELETE >=> routeWithLogging "DELETE" path func

    let deleteRouteF path func =
        DELETE >=> routeFWithLogging "DELETE" path func

    let routes : HttpFunc -> HttpContext -> HttpFuncResult = choose [
        getRoute "/status" (fun next ctx -> sendString (Ok "Server is up and running") next ctx)
    ]