namespace TestLibrary

open System.Text
open System.Security.Cryptography
open System

module Security =

    let getShaPassword (plainText: string) =
        let shaM = new SHA512Managed()
        let hash = plainText
                   |> Encoding.UTF8.GetBytes
                   |> shaM.ComputeHash
                   |> BitConverter.ToString
        hash.Replace("-", "").ToLower()
