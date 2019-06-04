module UserTestDataProvider

open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open Newtonsoft.Json
open Consilium.UserTypes
open TestLibrary

let createUser credentials =
    let request = JsonConvert.SerializeObject credentials

    Http.Request
         ( Base.config.TestServerUrl + "/users",
           headers = [ ContentType HttpContentTypes.Json ],
           body = TextRequest request ) |> ignore

    credentials

let createRandomUser _ =
    let credentials = { email = Randomize.email ()
                        password = Randomize.password () }

    createUser credentials

let signInWithCredentials (credentials: Credentials) =
    let request = JsonConvert.SerializeObject credentials

    Http.Request
         ( Base.config.TestServerUrl + "/authenticate",
           headers = [ ContentType HttpContentTypes.Json ],
           body = TextRequest request ) 

let signInWithRandomUser<'a> =
    signInWithCredentials << createRandomUser

let getTokenForUser credentials =
    createUser credentials |> ignore

    let response = signInWithCredentials credentials

    match response.Body with
    | Text token -> token
    | Binary _ -> ""
