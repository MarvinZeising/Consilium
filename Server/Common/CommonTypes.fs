namespace Consilium

open System

module CommonTypes =

    type Error =
         | EmailAlreadyExists
         | EmailRequired
         | EmailInvalid
         | LanguageRequired
         | LanguageNotAvailable
         | AuthenticationFailed
         | ServerException of Exception

    let mapErrorCode error =
        match error with
        | EmailAlreadyExists -> 400
        | EmailRequired -> 400
        | EmailInvalid -> 400
        | LanguageRequired -> 400
        | LanguageNotAvailable -> 400
        | AuthenticationFailed -> 401
        | ServerException _ -> 500
