module ProjectValidation

open Xunit
open Consilium
open CommonTypes
open TestLibrary

[<Fact>]
let ``validate should return name`` () =
    let result = "My super Project" |> ProjectValidation.validateName
    result |> shouldSucceedWith "My super Project"

[<Fact>]
let ``validate name is not empty`` () =
    let result = "" |> ProjectValidation.validateName
    result |> shouldContainError NameRequired

[<Fact>]
let ``validate name is not more than 40 characters`` () =
    let result = "waytoomanycharacters.morethan40aretoomuch"
                 |> ProjectValidation.validateName
    result |> shouldContainError NameLength
