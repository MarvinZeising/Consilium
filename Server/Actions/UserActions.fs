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
            // TODO: canonicalize email
            let! userId = context |> getUserId
            return! updateEmail (userId, validatedRequest.email)
        }

    let updateLanguage context request =
        result {
            let! validatedRequest = LanguageValidation.validateLanguage request
            let! userId = context |> getUserId
            return! updateLanguage (userId, validatedRequest.language)
        }

    let updatePassword context request =
        result {
            let! userId = context |> getUserId
            let! user = getUserById userId
            let! validatedRequest = PasswordValidation.validatePassword (request, user)
            let! enhancedRequest = switch PasswordValidation.hashPassword validatedRequest
            return! updatePassword (userId, enhancedRequest.newPassword)
        }
