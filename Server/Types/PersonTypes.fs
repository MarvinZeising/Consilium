namespace Consilium

module PersonTypes =

    type UpdateNameRequest =
         { PersonId: string
           Firstname: string
           Lastname: string }

    type Person =
         { Id: string
           Firstname: string
           Lastname: string }
