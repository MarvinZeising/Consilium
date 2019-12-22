namespace Consilium

module ProjectParticipationHandlers =

    open Giraffe
    open System
    open Microsoft.AspNetCore.Http
    open CommonValidation
    open ProjectParticipationTypes
    open ProjectParticipationDatabase
    open Actions

    let getProjectParticipationsForPerson (personId: string) (ctx: HttpContext) =
        result {
            let! userId = Authentication.getUserId ctx
            // TODO: check permissions

            return! getProjectParticipations personId ctx
        }

    let invitePersonToProject (request: InviteToProjectRequest) (ctx: HttpContext) =
        result {
            let newId = ShortGuid.fromGuid(Guid.NewGuid())

            let validatedPersonId = request.PersonId //|> validatePersonName
            let validatedProjectId = request.ProjectId //|> validatePersonName
            let! userId = Authentication.getUserId ctx
            // TODO: check permissions
            // TODO: check for duplicates

            return insertProjectParticipation {
                Id = newId
                PersonId = validatedPersonId
                ProjectId = validatedProjectId
                CreatedDate = DateTime.UtcNow.ToHtmlString()
                Status = ProjectParticipationStatus.Invited.ToString() }
        }

    let requestProjectParticipation (request: RequestProjectParticipationRequest) (ctx: HttpContext) =
        result {
            let newId = ShortGuid.fromGuid(Guid.NewGuid())

            let validatedPersonId = request.PersonId //|> validatePersonName
            let validatedProjectId = request.ProjectId //|> validatePersonName
            let! userId = Authentication.getUserId ctx
            // TODO: check permissions
            // TODO: check for duplicates

            return insertProjectParticipation {
                Id = newId
                PersonId = validatedPersonId
                ProjectId = validatedProjectId
                CreatedDate = DateTime.UtcNow.ToHtmlString()
                Status = ProjectParticipationStatus.Requested.ToString() }
        }

    let updateProjectParticipationRequest (request: UpdateProjectParticipationRequest) (ctx: HttpContext) =
        result {
            let validatedStatus = request.Status.ToString() //|> validatePersonName
            let! userId = Authentication.getUserId ctx
            // TODO: check permissions

            return updateProjectParticipation {
                Id = request.Id
                Status = validatedStatus }
        }

    let deleteProjectParticipation (projectParticipationId: string) (ctx: HttpContext) =
        result {
            let! userId = Authentication.getUserId ctx
            // TODO: check permissions

            return deleteProjectParticipation projectParticipationId
        }
