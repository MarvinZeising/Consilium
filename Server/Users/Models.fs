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

type UserFind = string -> User[]

type SignUp = Credentials -> bool

type SignIn = Credentials -> string option
