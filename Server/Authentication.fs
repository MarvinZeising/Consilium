module Authentication

open Giraffe
open System
open System.Text
open System.Security.Claims
open System.IdentityModel.Tokens.Jwt
open Microsoft.IdentityModel.Tokens
open Microsoft.AspNetCore.Authentication.JwtBearer

let secret = "!H^EU(-=m{VNkM'ECh'V7X7S:q4V"

let authorize<'T> =
    requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme)

let generateToken email =
    let claims = [|
        Claim(JwtRegisteredClaimNames.Email, email)
        Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    |]

    let expires = Nullable(DateTime.UtcNow.AddHours(1.0))
    let notBefore = Nullable(DateTime.UtcNow)
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

let getEmailFromToken token =
    let handler = new JwtSecurityTokenHandler()
    let claims = handler.ReadJwtToken(token).Claims
    let emailClaim = claims |> Seq.find (fun c -> c.Type = JwtRegisteredClaimNames.Email)
    emailClaim.Value

let hash (password : string) : string =
    let salt = CryptSharp.Crypter.Blowfish.GenerateSalt()

    CryptSharp.Crypter.Blowfish.Crypt(password, salt)

let verify (password : string) (hash : string) : bool =
    CryptSharp.Crypter.CheckPassword(password, hash)
