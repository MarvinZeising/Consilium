module UserTestDataProvider

open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open Newtonsoft.Json
open Consilium.UserTypes
open TestLibrary

let createRandomUser _ =
    let credentials = { email = Randomize.email ()
                        password = Randomize.password () }

    let request = JsonConvert.SerializeObject credentials

    Http.Request
         ( Base.config.TestServerUrl + "/users",
           headers = [ ContentType HttpContentTypes.Json ],
           body = TextRequest request ) |> ignore

    credentials

let signInWithCredentials credentials =
    let request = JsonConvert.SerializeObject credentials

    Http.Request
         ( Base.config.TestServerUrl + "/authenticate",
           headers = [ ContentType HttpContentTypes.Json ],
           body = TextRequest request ) 

let signInWithRandomUser =
    signInWithCredentials createRandomUser
