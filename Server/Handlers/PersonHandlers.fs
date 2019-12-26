namespace Consilium

module PersonHandlers =

    open Giraffe
    open System
    open Microsoft.AspNetCore.Http
    open CommonValidation
    open PersonValidation
    open PersonTypes
    open PersonDatabase
    open Actions

    let private validatePersonName = validateName 20

    let getPerson personId (ctx: HttpContext) =
        result {
            let! userId = Authentication.getUserId ctx

            return! getPerson personId userId ctx
        }

    let getPersons (ctx: HttpContext) =
        result {
            let! userId = Authentication.getUserId ctx

            return! getPersons userId ctx
        }

    let createPerson (request: CreatePersonRequest) (ctx: HttpContext) =
        result {
            let newId = ShortGuid.fromGuid(Guid.NewGuid())

            let! validatedFirstname = request.Firstname |> validatePersonName
            let! validatedLastname = request.Lastname |> validatePersonName
            let! validatedGender = request.Gender |> validateGender
            let! userId = Authentication.getUserId ctx

            return insertPerson {
                Id = newId
                UserId = userId
                Firstname = validatedFirstname
                Lastname = validatedLastname
                Gender = validatedGender }
        }

    let updateGeneral (request: UpdateGeneralRequest) (ctx: HttpContext) =
        result {
            let! validatedFirstname = request.Firstname |> validatePersonName
            let! validatedLastname = request.Lastname |> validatePersonName
            let! validatedGender = request.Gender |> validateGender
            let! userId = Authentication.getUserId ctx

            return updateGeneral {
                Id = request.Id
                UserId = userId
                Firstname = validatedFirstname
                Lastname = validatedLastname
                Gender = validatedGender }
        }

    let deletePerson personId (ctx: HttpContext) =
        result {
            return deletePerson personId
        }
