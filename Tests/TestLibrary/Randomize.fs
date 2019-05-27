namespace TestLibrary

open System

module Randomize =

    let string =
        let chars = "abcdefghijklmnopqrstuvwuxyz"
        let charsLen = chars.Length
        let random = System.Random()

        fun len ->
            let randomChars = [| for i in 0..len -> chars.[random.Next(charsLen)] |]
            new System.String(randomChars)

    let email = fun _ ->
        let name = string 10
        let domain = string 10
        name + "@" + domain + ".com"

    let password = fun _ ->
        Security.getShaPassword <| string 8 
