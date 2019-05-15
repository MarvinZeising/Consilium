namespace Consilium

open System.Text.RegularExpressions
open CommonLibrary 
open CommonTypes
open UserTypes
open Validation

module EmailValidation = 

    let private validateRequired email =
       if email = "" then Error [EmailRequired]
       else Ok email

    let private validateFormat email =
       if Regex.IsMatch(email, ".+@.+") then Ok email
       else Error [EmailInvalid]

    let validateEmail = 
        validateRequired
        &&& validateFormat

    let canonicalizeEmail (email : string) =
       email.Trim().ToLower()

module LanguageValidation =


    let availableLanguages = [|"de-DE";"en-US"|]

    let private validateRequired language =
       if language = "" then Error [LanguageRequired]
       else Ok language

    let private validateAvailable language =
       if Array.contains language availableLanguages then Ok language
       else Error [LanguageNotAvailable]

    let validateLanguage = 
        validateRequired
        &&& validateAvailable

module PasswordValidation =

    let private validatePasswordCorrect (request, user) =
        if Authentication.verify request.oldPassword user.Password then Ok request
        else Error [PasswordWrong]

    let private validateLength (request, _) =
        if request.newPassword.Length = 128 then Ok request
        else Error [PasswordInvalid]

    let validatePassword =
        validateLength
        &&& validatePasswordCorrect

    let hashPassword request =
        { request with newPassword = request.newPassword |> Authentication.hash}
