module Crypto

let hash (password : string) : string =
    let salt = CryptSharp.Crypter.Blowfish.GenerateSalt()

    CryptSharp.Crypter.Blowfish.Crypt(password, salt)

let verify (password : string) (hash : string) : bool =
    CryptSharp.Crypter.CheckPassword(password, hash)
