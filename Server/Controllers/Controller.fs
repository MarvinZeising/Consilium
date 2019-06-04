namespace Consilium

module Controller =

    open Giraffe
    open CommonTypes

    let private returnError errors =
        errors
        |> List.head
        |> mapErrorCode
        |> setStatusCode
        >=> json (List.map string errors)

    let sendJson (result : Result<'a,Error list>) =
        result |> CommonLibrary.either ResponseWriters.json returnError

    let sendString (result : Result<string,Error list>) =
        result |> CommonLibrary.either ResponseWriters.htmlString returnError
