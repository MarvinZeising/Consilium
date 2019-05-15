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
        let checkUserNotFoundError ex =
            if List.head ex = UserNotFound
            then succeed true
            else fail ex

        EmailValidation.validateEmail
        >=> switch EmailValidation.canonicalizeEmail
        >=> getUserByEmail
        >> either (fun _ -> succeed false) checkUserNotFoundError

    let private isCorrectPassword password (user : User) =
        if Authentication.verify password user.Password
        then succeed user
        else fail [AuthenticationFailed]

    let signIn<'a> (credentials : Credentials) =
        credentials.email
        |> (getUserByEmail
           >=> (isCorrectPassword credentials.password)
           >=> switch (fun user -> Authentication.generateToken user.Id user.Email))

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
