namespace Consilium

module CommonTypes =

    open System

    type Error =
         | UserNotFound
         | PersonNotFound
         | ProjectNotFound
         | TopicNotFound
         | NameRequired
         | NameLength
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
        | PersonNotFound -> 404
        | ProjectNotFound -> 404
        | TopicNotFound -> 404
        | NameRequired -> 400
        | NameLength -> 400
        | EmailAlreadyExists -> 400
        | EmailRequired -> 400
        | EmailInvalid -> 400
        | LanguageRequired -> 400
        | LanguageNotAvailable -> 400
        | PasswordInvalid -> 400
        | PasswordWrong -> 400
        | AuthenticationFailed -> 401
        | ServerException _ -> 500
