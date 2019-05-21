namespace Consilium

module ProjectTypes =

    type UpdateGeneralRequest =
         { Id: string
           Name: string
           Email: string }

    type CreateProjectRequest =
         { Name: string
           Email: string }

    type Project =
         { Id: string
           Name: string
           Email: string }
