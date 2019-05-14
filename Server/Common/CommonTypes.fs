namespace Consilium

open System

module CommonTypes =

    type Error =
         | EmailAlreadyExists
         | EmailBlank
         | EmailInvalid
         | AuthenticationFailed
         | ServerException of Exception

    let mapErrorCode error =
        match error with
        | EmailAlreadyExists -> 400
        | EmailBlank -> 400
        | EmailInvalid -> 400
        | AuthenticationFailed -> 401
        | ServerException _ -> 500
