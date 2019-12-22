namespace Consilium

module ProjectParticipationController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open ProjectParticipationTypes
    open ProjectParticipationHandlers
    open Authentication
    open Controller

    let routes : HttpFunc -> HttpContext -> HttpFuncResult = authorize >=> choose [

        GET >=> routef "/project-participations/%s" (handlePayload getProjectParticipationsForPerson)

        POST >=> route "/project-participations/invite"
             >=> bindJson<InviteToProjectRequest> (handlePayload invitePersonToProject)

        POST >=> route "/project-participations/request"
             >=> bindJson<RequestProjectParticipationRequest> (handlePayload requestProjectParticipation)

        PUT >=> route "/project-participations"
            >=> bindJson<UpdateProjectParticipationRequest> (handlePayload updateProjectParticipationRequest)

        DELETE >=> routef "/project-participations/%s" (handlePayload deleteProjectParticipation)

    ]
