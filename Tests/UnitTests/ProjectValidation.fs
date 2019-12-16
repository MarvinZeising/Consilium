module ProjectValidation

open Xunit
open Consilium
open CommonTypes
open CommonValidation
open TestLibrary

[<Fact>]
let ``validate should return name`` () =
    let result = "My super Project" |> validateName
    result |> shouldSucceedWith "My super Project"

[<Fact>]
let ``validate name is not empty`` () =
    let result = "" |> validateName
    result |> shouldContainError NameRequired

[<Fact>]
let ``validate name is not more than 40 characters`` () =
    let result = "waytoomanycharacters.morethan40aretoomuch"
                 |> validateName
    result |> shouldContainError NameLength
