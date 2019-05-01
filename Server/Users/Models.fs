namespace Users

type User =
    {
        Id: string
        Username: string
        Password: string
    }

type UserSave = User -> User

type UserCriteria =
    | All

type UserFind = UserCriteria -> User[]

type UserFindByUsername = string -> User option
