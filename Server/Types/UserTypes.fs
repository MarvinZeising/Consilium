namespace Consilium

module UserTypes =

    type UpdateEmailRequest = { email: string }

    type UpdateLanguageRequest = { language: string }

    type UpdatePasswordRequest =
        { oldPassword: string
          newPassword: string }

    type User =
        {
            Id: string
            Email: string
            Password: string
            Language: string
        }

    type UserFind = string -> User option

    type UserDelete = string -> unit option
