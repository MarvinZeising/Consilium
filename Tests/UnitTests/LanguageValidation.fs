module LanguageValidation

open Xunit
open Consilium
open CommonTypes
open TestLibrary

[<Fact>]
let ``validate language is valid`` () =
    let result = "en-US" |> LanguageValidation.validate
    result |> shouldSucceedWith "en-US"

[<Fact>]
let ``validate language is not empty`` () =
    let result = "" |> LanguageValidation.validate
    result |> shouldContainError LanguageRequired

[<Fact>]
let ``validate language is not unknown`` () =
    let result = "unknown" |> LanguageValidation.validate
    result |> shouldContainError LanguageNotAvailable
