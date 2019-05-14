namespace Consilium

module EmailValidation = 

    open System.Text.RegularExpressions
    open CommonLibrary 
    open CommonTypes
    open UserTypes
    open Validation

    let private validateRequired input =
       if input.email = "" then fail [EmailRequired]
       else succeed input

    let private validateFormat input =
       if Regex.IsMatch(input.email, ".+@.+") then succeed input
       else fail [EmailInvalid]

    let validateEmail = 
        validateRequired
        &&& validateFormat

    let canonicalizeEmail input =
       { input with email = input.email.Trim().ToLower() }
