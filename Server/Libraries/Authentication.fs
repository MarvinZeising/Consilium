namespace Consilium

module Authentication =

    open Giraffe
    open System
    open System.Text
    open System.Security.Claims
    open System.IdentityModel.Tokens.Jwt
    open Microsoft.IdentityModel.Tokens
    open Microsoft.AspNetCore.Authentication.JwtBearer
    open Microsoft.AspNetCore.Http
    open CommonTypes
    open CommonLibrary

    let secret = "!H^EU(-=m{VNkM'ECh'V7X7S:q4V"

    let authorize<'T> =
        requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)

    let generateToken id email =
        let claims = [|
            Claim(JwtRegisteredClaimNames.Sub, id)
            Claim(JwtRegisteredClaimNames.Email, email)
            Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        |]

        let expires = DateTime.UtcNow.AddDays(7.0) |> Nullable
        let notBefore = DateTime.UtcNow |> Nullable
        let securityKey = SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        let signingCredentials = SigningCredentials(key = securityKey, algorithm = SecurityAlgorithms.HmacSha256)

        let token =
            JwtSecurityToken(
                "consiliumapp.org",
                "consiliumapp.org",
                claims,
                notBefore,
                expires,
                signingCredentials)

        JwtSecurityTokenHandler().WriteToken(token)

    let getAuthorization (context: HttpContext) =
        context.TryGetRequestHeader "Authorization"

    let extractTokenFromAuthorization (authorization : string option) =
        authorization
        |> Option.map (fun token -> token.Replace("Bearer ", ""))

    let extractClaim claimType token =
        let handler = new JwtSecurityTokenHandler()
        handler.ReadJwtToken(token).Claims
            |> Seq.tryFind (fun c -> c.Type = claimType)
            |> Option.map (fun claim -> claim.Value)

    let getEmailFromToken = extractClaim JwtRegisteredClaimNames.Email
    let getIdFromToken = extractClaim JwtRegisteredClaimNames.Sub

    let getUserId<'a> =
        getAuthorization
        >> extractTokenFromAuthorization
        >> Option.bind getIdFromToken
        >> maybeSucceed [AuthenticationFailed]

    let hash (password : string) : string =
        let salt = CryptSharp.Crypter.Blowfish.GenerateSalt()

        CryptSharp.Crypter.Blowfish.Crypt(password, salt)

    let verify (password : string) (hash : string) : bool =
        CryptSharp.Crypter.CheckPassword(password, hash)

