namespace Consilium

open CommonLibrary 
open CommonTypes
open UserTypes
open Validation

module UserValidation =

    let validateLanguage language =
        match language with
        | "en-US" -> Ok "en-US"
        | "de-DE" -> Ok "de-DE"
        | "" -> Error [LanguageRequired]
        | _ -> Error [LanguageInvalid]

    let validateTheme theme =
        match theme with
        | "light" -> Ok "light"
        | "dark" -> Ok "dark"
        | "" -> Error [ThemeRequired]
        | _ -> Error [ThemeInvalid]

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
