namespace Consilium

module UserHandlers =

    open Giraffe
    open System
    open Microsoft.AspNetCore.Http
    open CommonValidation
    open UserValidation
    open UserTypes
    open UserDatabase
    open Actions

    let private validatePersonName = validateName 20

    let updateInterface (request: UpdateInterfaceRequest) (ctx: HttpContext) =
        result {
            let! validatedLanguage = request.Language |> validateLanguage
            let! validatedTheme = request.Theme |> validateTheme
            let! userId = Authentication.getUserId ctx

            return updateInterface {
                Id = userId
                Language = validatedLanguage
                Theme = validatedTheme }
        }
