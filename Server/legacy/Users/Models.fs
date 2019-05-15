namespace Users

type User =
    {
        Id: string
        Email: string
        Password: string
        Language: string
    }

type Credentials =
     {
         Email: string
         Password: string
     }

type SignUp = Credentials -> bool

type SignIn = Credentials -> string option
