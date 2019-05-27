module UserController

open Xunit
open Shouldly
open FSharp.Data
open TestLibrary

[<Fact>]
let ``GET /users/email-available should return true if available`` () =
    let result = Http.RequestString <| "http://localhost:5000/users/email-available/" + Randomize.email ()

    result.ShouldBe "true"

[<Fact>]
let ``GET /users/email-available should return false if not available`` () =
    let credentials = UserTestDataProvider.createRandomUser

    let result = Http.RequestString <| "http://localhost:5000/users/email-available/" + credentials.email

    result.ShouldBe "false"
