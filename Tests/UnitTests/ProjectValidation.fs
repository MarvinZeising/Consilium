module ProjectValidation

open Xunit
open Consilium
open CommonTypes
open CommonValidation
open TestLibrary

let private validateProjectName = validateName 40

[<Fact>]
let ``validate should return name`` () =
    let result = "My super Project" |> validateProjectName
    result |> shouldSucceedWith "My super Project"

[<Fact>]
let ``validate name is not empty`` () =
    let result = "" |> validateProjectName
    result |> shouldContainError NameRequired

[<Fact>]
let ``validate name is not more than 40 characters`` () =
    let result = "waytoomanycharacters.morethan40aretoomuch"
                 |> validateProjectName
    result |> shouldContainError NameLength
