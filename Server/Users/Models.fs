namespace Users

type User =
    {
        Id: string
        Email: string
        Password: string
    }

type Credentials =
     {
         Email: string
         Password: string
     }

type PasswordChange =
     {
         Id: string
         Old: string
         New: string
     }

type EmailChange =
     {
         Id: string
         Email: string
     }

type EmailAvailable = string -> bool

type UserFind = string -> User option

type UserDelete = string -> unit option

type SignUp = Credentials -> bool

type SignIn = Credentials -> string option

type UpdateEmail = EmailChange -> unit option

type UpdatePassword = PasswordChange -> unit option
