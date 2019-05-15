namespace Consilium

module UserTypes =

    type Credentials =
         { email: string
           password: string }

    type UpdateEmailRequest =
         { email: string }

    type UpdateLanguageRequest =
         { language: string }

    type UpdatePasswordRequest =
         { oldPassword: string
           newPassword: string }

    type User =
         { Id: string
           Email: string
           Password: string
           Language: string }
