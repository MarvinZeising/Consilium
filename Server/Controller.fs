module Controller

open Giraffe

let resultOr404 next context x =
    match x with
    | Some x -> json x next context
    | None -> setStatusCode 404 next context
