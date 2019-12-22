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

    [<CLIMutable>]
    type UpdateInterfaceRequest =
         { Id: string // filled server-side
           Language: string
           Theme: string }

    type User =
         { Id: string
           Email: string
           Password: string
           Language: string
           Theme: string }
