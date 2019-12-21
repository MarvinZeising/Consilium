namespace Consilium

module PersonTypes =

    [<CLIMutable>]
    type UpdateGeneralRequest =
         { Id: string
           UserId: string // filled server-side
           Firstname: string
           Lastname: string
           Gender: string }

    [<CLIMutable>]
    type CreatePersonRequest =
         { Firstname: string
           Lastname: string
           Gender: string }

    type Person =
         { Id: string
           UserId: string
           Firstname: string
           Lastname: string
           Gender: string }
