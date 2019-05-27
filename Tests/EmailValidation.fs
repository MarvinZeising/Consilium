module EmailValidation

open Xunit
open Consilium
open CommonTypes
open TestLibrary

[<Fact>]
let ``validate should canonicalize email`` () =
    let result = " tEsT@ExaMplE.COm  " |> EmailValidation.validate
    result |> shouldSucceedWith "test@example.com"

[<Fact>]
let ``validate email is not empty`` () =
    let result = "" |> EmailValidation.validate
    result |> shouldContainError EmailRequired

[<Fact>]
let ``validate email contains at-sign`` () =
    let result = "wrong" |> EmailValidation.validate
    result |> shouldContainError EmailInvalid

[<Fact>]
let ``validate email contains at-sign after character`` () =
    let result = "@wrong" |> EmailValidation.validate
    result |> shouldContainError EmailInvalid

[<Fact>]
let ``validate email contains dot`` () =
    let result = "wrong@address" |> EmailValidation.validate
    result |> shouldContainError EmailInvalid

[<Fact>]
let ``validate email contains dot after characters`` () =
    let result = "wrong@.com" |> EmailValidation.validate
    result |> shouldContainError EmailInvalid

[<Fact>]
let ``validate email contains dot before characters`` () =
    let result = "wrong@address." |> EmailValidation.validate
    result |> shouldContainError EmailInvalid
