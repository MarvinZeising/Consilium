namespace Consilium

module ProjectValidation =

    open CommonTypes
    open CommonLibrary
    open Validation
    open ProjectTypes

    let private validateRequired name =
       if name = "" then Error [NameRequired]
       else Ok name

    let private validateLength (name : string) =
       if name.Length > 40 then Error [NameLength]
       else Ok name

    let validateName =
        validateRequired
        &&& validateLength
