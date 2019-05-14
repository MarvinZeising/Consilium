namespace Consilium

open System

module CommonTypes =

    type Error =
         | UserNotFound
         | EmailAlreadyExists
         | EmailRequired
         | EmailInvalid
         | LanguageRequired
         | LanguageNotAvailable
         | PasswordInvalid
         | PasswordWrong
         | AuthenticationFailed
         | ServerException of Exception

    let mapErrorCode error =
        match error with
        | UserNotFound -> 404
        | EmailAlreadyExists -> 400
        | EmailRequired -> 400
        | EmailInvalid -> 400
        | LanguageRequired -> 400
        | LanguageNotAvailable -> 400
        | PasswordInvalid -> 400
        | PasswordWrong -> 400
        | AuthenticationFailed -> 401
        | ServerException _ -> 500
