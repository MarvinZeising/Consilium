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

    let private canonicalizeEmail (email : string) =
       email.Trim().ToLower()

    let validate = 
        validateRequired
        &&& validateFormat
        >=> switch canonicalizeEmail

module LanguageValidation =

    let availableLanguages = [|"de-DE";"en-US"|]

    let private validateRequired language =
       if language = "" then Error [LanguageRequired]
       else Ok language

    let private validateAvailable language =
       if Array.contains language availableLanguages then Ok language
       else Error [LanguageNotAvailable]

    let validate = 
        validateRequired
        &&& validateAvailable

module PasswordValidation =

    type InputModel =
         { oldPassword: string
           newPassword: string
           passwordHash: string }

    let private validatePasswordCorrect model =
        if Authentication.verify model.oldPassword model.passwordHash then Ok model
        else Error [PasswordWrong]

    let private validateLength model =
        if model.newPassword.Length = 128 then Ok model
        else Error [PasswordInvalid]

    let private hashPassword model =
        model.newPassword |> Authentication.hash

    let validate = (fun (request : UpdatePasswordRequest) user ->
        let model = { oldPassword=request.oldPassword
                      newPassword=request.newPassword
                      passwordHash=user.Password}

        model |> (validatePasswordCorrect
                  &&& validateLength
                  >=> switch hashPassword))
