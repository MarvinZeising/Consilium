namespace Consilium

open System.Text.RegularExpressions
open CommonLibrary 
open CommonTypes
open UserTypes
open Validation

module EmailValidation = 

    let private validateRequired request =
       if request.email = "" then fail [EmailRequired]
       else succeed request

    let private validateFormat request =
       if Regex.IsMatch(request.email, ".+@.+") then succeed request
       else fail [EmailInvalid]

    let validateEmail = 
        validateRequired
        &&& validateFormat

    let canonicalizeEmail request =
       { request with email = request.email.Trim().ToLower() }

module LanguageValidation =


    let availableLanguages = [|"de-DE";"en-US"|]

    let private validateRequired request =
       if request.language = "" then fail [LanguageRequired]
       else succeed request

    let private validateAvailable request =
       if Array.contains request.language availableLanguages then succeed request
       else fail [LanguageNotAvailable]

    let validateLanguage = 
        validateRequired
        &&& validateAvailable

module PasswordValidation =

    let private validatePasswordCorrect (request, user) =
        if Authentication.verify request.oldPassword user.Password then succeed request
        else fail [PasswordWrong]

    let private validateLength (request, _) =
        if request.newPassword.Length = 128 then succeed request
        else fail [PasswordInvalid]

    let validatePassword =
        validateLength
        &&& validatePasswordCorrect

    let hashPassword request =
        { request with newPassword = request.newPassword |> Authentication.hash}
