module LanguageValidation

open Xunit
open Consilium
open CommonTypes
open UserValidation
open TestLibrary

[<Fact>]
let ``validate language is valid`` () =
    let result = "en-US" |> validateLanguage
    result |> shouldSucceedWith "en-US"

[<Fact>]
let ``validate language is not empty`` () =
    let result = "" |> validateLanguage
    result |> shouldContainError LanguageRequired

[<Fact>]
let ``validate language is not unknown`` () =
    let result = "unknown" |> validateLanguage
    result |> shouldContainError LanguageInvalid
