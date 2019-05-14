namespace Consilium

open Validation

module EmailValidation = 

    open CommonLibrary 
    open CommonTypes
    open UserTypes
    open System.Text.RegularExpressions

    let validateEmailRequired input =
       if input.email = "" then fail [EmailBlank]
       else succeed input

    let validateEmailFormat input =
       if Regex.IsMatch(input.email, ".+@.+") then succeed input
       else fail [EmailInvalid]

    let combinedEmailValidation = 
        validateEmailRequired
        &&& validateEmailFormat

    let canonicalizeEmail input =
       { input with email = input.email.Trim().ToLower() }
