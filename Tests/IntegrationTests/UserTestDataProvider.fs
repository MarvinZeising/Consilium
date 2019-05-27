module UserTestDataProvider

open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open Newtonsoft.Json
open TestLibrary
open Consilium.UserTypes

let createRandomUser =
    let credentials = { email = Randomize.email ()
                        password = Randomize.password () }

    let request = JsonConvert.SerializeObject(credentials);

    Http.Request
         ( "http://localhost:5000/users",
           headers = [ ContentType HttpContentTypes.Json ],
           body = TextRequest request ) |> ignore

    credentials
