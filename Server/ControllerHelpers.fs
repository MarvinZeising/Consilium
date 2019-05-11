module ControllerHelpers

open Giraffe

let resultOrStatusCode code next context x =
    match x with
    | Some x -> json x next context
    | None -> setStatusCode code next context
