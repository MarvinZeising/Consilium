namespace Consilium

/// ===========================================
/// All the use cases or services in one place
/// ===========================================
module UserActions =

    open CommonLibrary
    open DomainTypes
    open UserRepository

    // HttpContext -> Result string ?
    let getId<'a> =
        Authentication.getAuthorization
        >> Authentication.extractTokenFromAuthorization
        >> Option.bind Authentication.getIdFromToken
        >> maybeSucceed [AuthenticationFailed]

    let result = new ResultBuilder()

    // real logic - end of mocks

    let handleEmailUpdate context request =
        result {
            let! validatedRequest = Validation.combinedEmailValidation request |> Logger.log
            let! userId = context |> getId |> Logger.log
            return! updateDatabase (userId, validatedRequest.email)
        }
