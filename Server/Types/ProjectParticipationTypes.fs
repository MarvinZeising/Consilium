namespace Consilium

module ProjectParticipationTypes =

    type ProjectParticipationStatus =
    | Invited
    | Requested
    | Active
    | Inactive

    [<CLIMutable>]
    type InviteToProjectRequest =
         { Date: string // filled server-side
           PersonId: string
           ProjectId: string }

    [<CLIMutable>]
    type RequestProjectParticipationRequest =
         { Date: string // filled server-side
           PersonId: string
           ProjectId: string }

    [<CLIMutable>]
    type UpdateProjectParticipationRequest =
         { Id: string
           Status: string }

    type ProjectParticipation =
         { Id: string
           PersonId: string
           ProjectId: string
           CreatedDate: string
           Status: string }
