namespace Consilium

module UserActions =

    open CommonLibrary
    open CommonTypes
    open EmailValidation
    open UserRepository

    // HttpContext -> Result string ?
    let getId<'a> =
        Authentication.getAuthorization
        >> Authentication.extractTokenFromAuthorization
        >> Option.bind Authentication.getIdFromToken
        >> maybeSucceed [AuthenticationFailed]

    let result = new ResultBuilder()

    // real logic - end of mocks

    let updateEmail context request =
        result {
            let! validatedRequest = combinedEmailValidation request
            let! userId = context |> getId
            return! updateDatabase (userId, validatedRequest.email)
        }
