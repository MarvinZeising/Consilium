namespace Consilium

/// ===========================================
/// Validation functions
/// ===========================================
module Validation = 

    open CommonLibrary 
    open DomainTypes
    open System.Text.RegularExpressions

    let validateEmailRequired input =
       if input.email = "" then fail [EmailBlank]
       else succeed input

    let validateEmailFormat input =
       if Regex.IsMatch(input.email, ".+@.+") then succeed input
       else fail [EmailInvalid]

    // create a "plus" function for validation functions
    let (&&&) v1 v2 = 
        let addSuccess r1 r2 = r1 // return first
        let addFailure s1 s2 = s1 @ s2  // concat
        plus addSuccess addFailure v1 v2 

    let combinedEmailValidation = 
        validateEmailRequired
        &&& validateEmailFormat

    let canonicalizeEmail input =
       { input with email = input.email.Trim().ToLower() }
