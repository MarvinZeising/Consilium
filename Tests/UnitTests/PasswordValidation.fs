module PasswordValidation

open Xunit
open Consilium
open CommonTypes
open UserTypes
open TestLibrary
open System.Security.Cryptography
open System.Text
open System

let private shaM = new SHA512Managed()

let private getPasswordData (plainText: string) =
    let password = Security.getShaPassword plainText
    let encryptedPassword = Authentication.hash password
    (password, encryptedPassword)

[<Fact>]
let ``validate password is sha512`` () =
    let (_, encryptedPassword) = getPasswordData "password123"

    let request =
        { oldPassword = encryptedPassword
          newPassword = "not enough characters for a sha512" }

    let user =
        { Id = ""
          Email = ""
          Password = encryptedPassword
          Language = "" }

    let result = PasswordValidation.validate request user
    result |> shouldContainError PasswordInvalid

[<Fact>]
let ``validate old password equals user password`` () =
    let (_, encryptedPassword) = getPasswordData "password123"
    let (_, encryptedWrongPassword) = getPasswordData "password1234"

    let request =
        { oldPassword = encryptedWrongPassword
          newPassword = "" }

    let user =
        { Id = ""
          Email = ""
          Password = encryptedPassword
          Language = "" }

    let result = PasswordValidation.validate request user
    result |> shouldContainError PasswordWrong

[<Fact>]
let ``validate should return hashed new password`` () =
    let (oldPassword, encryptedOldPassword) = getPasswordData "password123"
    let (newPassword, _) = getPasswordData "newPassword123"

    let request =
        { oldPassword = oldPassword
          newPassword = newPassword }

    let user =
        { Id = ""
          Email = ""
          Password = encryptedOldPassword
          Language = "" }

    let result = PasswordValidation.validate request user
    result |> shouldSucceedDoing (fun x -> Authentication.verify newPassword x)
