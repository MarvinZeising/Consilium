namespace Consilium

module LanguageValidation =

    open CommonLibrary 
    open CommonTypes
    open UserTypes
    open Validation

    let availableLanguages = [|"de-DE";"en-US"|]

    let private validateRequired input =
       if input.language = "" then fail [LanguageRequired]
       else succeed input

    let private validateAvailable input =
       if Array.contains input.language availableLanguages then succeed input
       else fail [LanguageNotAvailable]

    let validateLanguage = 
        (validateRequired >> Logger.log)
        &&& (validateAvailable >> Logger.log)
