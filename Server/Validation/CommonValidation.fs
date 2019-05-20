namespace Consilium

open System.Text.RegularExpressions
open CommonLibrary 
open CommonTypes
open Validation

module EmailValidation = 

    let private validateRequired email =
       if email = "" then Error [EmailRequired]
       else Ok email

    let private validateFormat email =
       if Regex.IsMatch(email, ".+@.+") then Ok email
       else Error [EmailInvalid]

    let private canonicalizeEmail (email : string) =
       email.Trim().ToLower()

    let validate = 
        validateRequired
        &&& validateFormat
        >=> switch canonicalizeEmail
