namespace Consilium

module UserActions =

    open CommonLibrary
    open CommonTypes
    open UserRepository

    let private result = new ResultBuilder()

    let private getUserId<'a> =
        Authentication.getAuthorization
        >> Authentication.extractTokenFromAuthorization
        >> Option.bind Authentication.getIdFromToken
        >> maybeSucceed [AuthenticationFailed]

    let getUser<'a> =
        getUserId >=> getUserById

    let isEmailAvailable email =
        getUserByEmail email |> bind (switch Option.isNone)

    let updateEmail context request =
        result {
            let! validatedRequest = EmailValidation.validateEmail request
            let! enhancedRequest = switch EmailValidation.canonicalizeEmail validatedRequest
            let! userId = context |> getUserId
            return! updateEmail (userId, enhancedRequest.email)
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
