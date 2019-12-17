namespace Consilium

module PersonTypes =

    [<CLIMutable>]
    type UpdateNameRequest =
         { PersonId: string
           Firstname: string
           Lastname: string }

    [<CLIMutable>]
    type CreatePersonRequest =
         { Firstname: string
           Lastname: string }

    type Person =
         { Id: string
           UserId: string
           Firstname: string
           Lastname: string }
