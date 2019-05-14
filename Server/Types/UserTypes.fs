namespace Consilium

module UserTypes =

    type UpdateEmailRequest =
         {
            email: string
         }

    type User =
        {
            Id: string
            Email: string
            Password: string
            Language: string // TODO: restrict to available languages?
        }

    type EmailChange =
         {
             Id: string
             Email: string
         }

    type UserFind = string -> User option

    type UserDelete = string -> unit option

    type UpdateEmail = EmailChange -> unit option
