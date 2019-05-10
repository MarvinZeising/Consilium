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

type EmailAvailable = string -> bool

type UserFind = string -> User option

type UserDelete = string -> unit option

type SignUp = Credentials -> bool

type SignIn = Credentials -> string option
