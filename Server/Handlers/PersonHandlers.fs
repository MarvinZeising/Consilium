namespace Consilium

module PersonHandlers =

    open Giraffe
    open System
    open Microsoft.AspNetCore.Http
    open CommonValidation
    open PersonTypes
    open PersonDatabase
    open Actions

    let getPersons (ctx: HttpContext) =
        result {
            let! userId = Authentication.getUserId ctx

            return! getPersons userId ctx
        }

    let createPerson (request: CreatePersonRequest) (ctx: HttpContext) =
        result {
            let newId = ShortGuid.fromGuid(Guid.NewGuid())

            let! validatedFirstname = request.Firstname |> validateName
            let! validatedLastname = request.Lastname |> validateName
            let! userId = Authentication.getUserId ctx

            let person = { Id = newId
                           UserId = userId
                           Firstname = validatedFirstname
                           Lastname = validatedLastname }

            return insertPerson person
        }

    let deletePerson personId (next:HttpFunc) (ctx: HttpContext) =
        result {
            deletePerson personId |> ignore
            return! Successful.OK "asdf"
        }
