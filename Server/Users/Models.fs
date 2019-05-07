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

type UserSave = User -> User

type UserCriteria =
    | Username of string
    | All

type EmailAvailable = string -> bool

type UserFind = UserCriteria -> User[]

type Authenticate = Credentials -> User option
