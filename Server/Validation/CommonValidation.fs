namespace Consilium

open System.Text.RegularExpressions
open CommonLibrary
open CommonTypes
open Validation

module CommonValidation =

    let private validateRequired (error: Error) value =
       if value = "" then Error [error]
       else Ok value

    let private validateEmailFormat email =
       if Regex.IsMatch(email, ".+@.+\..+") then Ok email
       else Error [EmailInvalid]

    let private canonicalizeEmail (email : string) =
       email.Trim().ToLower()

    let private validateLength maxLength (name : string) =
       if name.Length > maxLength then Error [NameLength]
       else Ok name

    let validateEmail =
        validateRequired EmailRequired
        &&& validateEmailFormat
        >=> switch canonicalizeEmail

    let validateName maxLength =
        validateRequired NameRequired
        &&& validateLength maxLength
