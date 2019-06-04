module UserController

open Xunit
open Shouldly
open FSharp.Data
open TestLibrary
open FSharp.Data.HttpRequestHeaders
open Newtonsoft.Json
open Consilium.UserTypes

[<Fact>]
let ``POST /users should create a new user`` () =
    UserTestDataProvider.createRandomUser () |> ignore

[<Fact>]
let ``GET /users/email-available should return true if available`` () =
    let result = Http.RequestString <| Base.config.TestServerUrl + "/users/email-available/" + Randomize.email ()

    result.ShouldBe "true"

[<Fact>]
let ``GET /users/email-available should return false if not available`` () =
    let credentials = UserTestDataProvider.createRandomUser ()

    let result = Http.RequestString <| Base.config.TestServerUrl + "/users/email-available/" + credentials.email

    result.ShouldBe "false"

[<Fact>]
let ``POST /authenticate should return a token`` () =
    let credentials = UserTestDataProvider.createRandomUser ()

    let response = UserTestDataProvider.signInWithCredentials credentials

    match response.Body with
    | Text token -> token.ShouldStartWith "ey"
    | Binary _ -> Assert.True(false)

[<Fact>]
let ``POST /users should create a user`` () =
    let credentials = UserTestDataProvider.createRandomUser ()

    let response = UserTestDataProvider.signInWithCredentials credentials

    response.StatusCode.ShouldBe 200

[<Fact>]
let ``GET /user should return the signed-in user`` () =
    let credentials = { email = Randomize.email ()
                        password = Randomize.password () }

    let token = UserTestDataProvider.getTokenForUser credentials

    let response = Http.RequestString
                        ( Base.config.TestServerUrl + "/user",
                          headers = [ ContentType HttpContentTypes.Json
                                      Authorization ("Bearer " + token) ])

    let user = JsonConvert.DeserializeObject<Consilium.UserTypes.User> response

    user.Email.ShouldBe credentials.email
