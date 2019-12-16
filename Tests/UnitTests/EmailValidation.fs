module EmailValidation

open Xunit
open Consilium
open CommonTypes
open CommonValidation
open TestLibrary

[<Fact>]
let ``validate should canonicalize email`` () =
    let result = " tEsT@ExaMplE.COm  " |> validateEmail
    result |> shouldSucceedWith "test@example.com"

[<Fact>]
let ``validate email is not empty`` () =
    let result = "" |> validateEmail
    result |> shouldContainError EmailRequired

[<Fact>]
let ``validate email contains at-sign`` () =
    let result = "wrong" |> validateEmail
    result |> shouldContainError EmailInvalid

[<Fact>]
let ``validate email contains at-sign after character`` () =
    let result = "@wrong" |> validateEmail
    result |> shouldContainError EmailInvalid

[<Fact>]
let ``validate email contains dot`` () =
    let result = "wrong@address" |> validateEmail
    result |> shouldContainError EmailInvalid

[<Fact>]
let ``validate email contains dot after characters`` () =
    let result = "wrong@.com" |> validateEmail
    result |> shouldContainError EmailInvalid

[<Fact>]
let ``validate email contains dot before characters`` () =
    let result = "wrong@address." |> validateEmail
    result |> shouldContainError EmailInvalid
