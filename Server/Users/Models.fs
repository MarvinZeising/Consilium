namespace Users

type User =
    {
        Id: string
        Username: string
        Password: string
    }

type Credentials =
     {
         Username: string
         Password: string
     }

type UserSave = User -> User

type UserCriteria =
    | Username of string
    | All

type UsernameAvailable = string -> bool

type UserFind = UserCriteria -> User[]

type UserAuthenticate = Credentials -> User option
