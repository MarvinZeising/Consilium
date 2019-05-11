namespace Consilium

/// ===========================================
/// Validation functions
/// ===========================================
module Validation = 

    open CommonLibrary 
    open DomainTypes

    let validate1 input =
       if input.name = "" then Failure "Name must not be blank"
       else Success input

    let validate2 input =
       if input.name.Length > 50 then Failure "Name must not be longer than 50 chars"
       else Success input

    let validate3 input =
       if input.email = "" then Failure "Email must not be blank"
       else Success input

    // create a "plus" function for validation functions
    let (&&&) v1 v2 = 
        let addSuccess r1 r2 = r1 // return first
        let addFailure s1 s2 = s1 + "; " + s2  // concat
        plus addSuccess addFailure v1 v2 

    let combinedValidation = 
        validate1 
        &&& validate2 
        &&& validate3 

    let canonicalizeEmail input =
       { input with email = input.email.Trim().ToLower() }

