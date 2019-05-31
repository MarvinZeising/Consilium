module UserTestDataProvider

open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open Newtonsoft.Json
open TestLibrary
open Consilium.UserTypes

let createRandomUser _ =
    let credentials = { email = Randomize.email ()
                        password = Randomize.password () }

    let request = JsonConvert.SerializeObject credentials

    Http.Request
         ( Config.serverUrl + "/users",
           headers = [ ContentType HttpContentTypes.Json ],
           body = TextRequest request ) |> ignore

    credentials

let signInWithCredentials credentials =
    let request = JsonConvert.SerializeObject credentials

    Http.Request
         ( Config.serverUrl + "/authenticate",
           headers = [ ContentType HttpContentTypes.Json ],
           body = TextRequest request ) 

let signInWithRandomUser =
    signInWithCredentials createRandomUser
