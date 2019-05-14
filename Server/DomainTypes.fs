namespace Consilium

open System

/// ===========================================
/// Global types for this project
/// ===========================================
module DomainTypes =

    type UpdateEmailRequest =
         {
            email: string
         }

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







    type User =
        {
            Id: string
            Email: string
            Password: string
            Language: string // TODO: restrict to available languages?
        }

    type EmailChange =
         {
             Id: string
             Email: string
         }

    type UserFind = string -> User option

    type UserDelete = string -> unit option

    type UpdateEmail = EmailChange -> unit option
