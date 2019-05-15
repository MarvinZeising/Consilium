namespace Consilium

module Controller =

    open Giraffe
    open CommonTypes

    let private returnError errors =
        mapErrorCode (List.head errors)
        |> setStatusCode
           >=> json (List.map string errors)

    let send (result : Result<'a,Error list>) =
        result
        |> CommonLibrary.either json returnError

