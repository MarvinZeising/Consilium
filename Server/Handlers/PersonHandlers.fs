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

            return insertPerson {
                Id = newId
                UserId = userId
                Firstname = validatedFirstname
                Lastname = validatedLastname }
        }

    let updateGeneral (request: UpdateGeneralRequest) (ctx: HttpContext) =
        result {
            let! validatedFirstname = request.Firstname |> validateName
            let! validatedLastname = request.Lastname |> validateName
            let! userId = Authentication.getUserId ctx

            return updateGeneral {
                Id = request.Id
                UserId = userId
                Firstname = validatedFirstname
                Lastname = validatedLastname }
        }

    let deletePerson personId (ctx: HttpContext) =
        result {
            return deletePerson personId
        }
