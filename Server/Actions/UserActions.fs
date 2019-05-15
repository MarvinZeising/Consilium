namespace Consilium

module UserActions =

    open CommonLibrary
    open CommonTypes
    open UserRepository
    open UserTypes

    let private result = new ResultBuilder()

    let private getUserId<'a> =
        Authentication.getAuthorization
        >> Authentication.extractTokenFromAuthorization
        >> Option.bind Authentication.getIdFromToken
        >> maybeSucceed [AuthenticationFailed]

    let getUser<'a> =
        getUserId >=> getUserById

    let isEmailAvailable =
        EmailValidation.validateEmail
        >=> switch EmailValidation.canonicalizeEmail
        >=> getUserByEmail
        >=> switch Option.isNone

    let updateEmail context (request : UpdateEmailRequest) =
        result {
            let! email = request.email
                         |> (EmailValidation.validateEmail
                             >=> switch EmailValidation.canonicalizeEmail)
            let! userId = context |> getUserId
            return! updateEmail (userId, email)
        }

    let updateLanguage context (request : UpdateLanguageRequest) =
        result {
            let! language = LanguageValidation.validateLanguage request.language
            let! userId = context |> getUserId
            return! updateLanguage (userId, language)
        }

    let updatePassword context request =
        result {
            let! userId = context |> getUserId
            let! user = getUserById userId
            let! validated = (request, user)
                             |> (PasswordValidation.validatePassword
                                 >=> switch PasswordValidation.hashPassword)
            return! updatePassword (userId, validated.newPassword)
        }

    let deleteUser<'a> =
        getUserId >=> deleteUser
