namespace Consilium

open CommonTypes

module PersonValidation =

    let validateGender gender =
        match gender with
        | "male" -> Ok "male"
        | "female" -> Ok "female"
        | _ -> Error [GenderInvalid]
