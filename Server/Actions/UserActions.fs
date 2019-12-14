namespace Consilium

module UserActions =

    open System
    open Giraffe
    open CommonLibrary
    open CommonTypes
    open CommonValidation
    open UserTypes
    open UserRepository
    open Actions

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
            then Ok true
            else Error ex

        validateEmail
        >=> getUserByEmail
        >> either (fun _ -> Ok false) checkUserNotFoundError

    let private isCorrectPassword password (user : User) =
        if Authentication.verify password user.Password
        then Ok user
        else Error [AuthenticationFailed]

    let signIn<'a> (credentials : Credentials) =
        credentials.email
        |> (getUserByEmail
           >=> (isCorrectPassword credentials.password)
           >=> switch (fun user -> Authentication.generateToken user.Id user.Email))

    let signUp (credentials : Credentials) =
        let createUser =
            let hashedPassword = Authentication.hash credentials.password
            let user = {
                Id = ShortGuid.fromGuid(Guid.NewGuid())
                Email = credentials.email
                Password = hashedPassword
                Language = "en-US"
            }
            insertUser user

        credentials.email
        |> (isEmailAvailable
            >=> boolEither createUser (Error [EmailAlreadyExists]))

    let updateEmail context (request : UpdateEmailRequest) =
        result {
            let! email = request.email |> validateEmail
            let! userId = context |> getUserId
            return! updateEmail (userId, email)
        }

    let updateLanguage context (request : UpdateLanguageRequest) =
        result {
            let! language = LanguageValidation.validate request.language
            let! userId = context |> getUserId
            return! updateLanguage (userId, language)
        }

    let updatePassword context request =
        result {
            let! user = context |> (getUserId >=> getUserById)
            let! passwordHash = PasswordValidation.validate request user
            return! updatePassword (user.Id, passwordHash)
        }

    let deleteUser<'a> =
        getUserId >=> deleteUser
