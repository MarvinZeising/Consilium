namespace Consilium

module UserActions =

    open CommonLibrary
    open CommonTypes
    open UserRepository

    // HttpContext -> Result string ?
    let getUserId<'a> =
        Authentication.getAuthorization
        >> Authentication.extractTokenFromAuthorization
        >> Option.bind Authentication.getIdFromToken
        >> maybeSucceed [AuthenticationFailed]

    let result = new ResultBuilder()

    // real logic - end of mocks

    let updateEmail context request =
        result {
            let! validatedRequest = EmailValidation.validateEmail request
            let! userId = context |> getUserId
            return! updateEmail (userId, validatedRequest.email)
        }

    let updateLanguage context request =
        result {
            let! validatedRequest = LanguageValidation.validateLanguage request
            let! userId = context |> getUserId
            return! updateLanguage (userId, validatedRequest.language)
        }
