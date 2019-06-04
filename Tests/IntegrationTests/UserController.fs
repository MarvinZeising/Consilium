module UserController

open Xunit
open Shouldly
open FSharp.Data
open TestLibrary

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
