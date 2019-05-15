namespace Consilium

open System.Text.RegularExpressions
open CommonLibrary 
open CommonTypes
open UserTypes
open Validation

module EmailValidation = 

    let private validateRequired email =
       if email = "" then fail [EmailRequired]
       else succeed email

    let private validateFormat email =
       if Regex.IsMatch(email, ".+@.+") then succeed email
       else fail [EmailInvalid]

    let validateEmail = 
        validateRequired
        &&& validateFormat

    let canonicalizeEmail (email : string) =
       email.Trim().ToLower()

module LanguageValidation =


    let availableLanguages = [|"de-DE";"en-US"|]

    let private validateRequired language =
       if language = "" then fail [LanguageRequired]
       else succeed language

    let private validateAvailable language =
       if Array.contains language availableLanguages then succeed language
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
