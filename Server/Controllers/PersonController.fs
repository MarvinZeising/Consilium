namespace Consilium

module PersonController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open PersonTypes
    open PersonHandlers
    open Controller

    let routes : HttpFunc -> HttpContext -> HttpFuncResult = choose [

        GET >=> routef "/persons/%s" (handlePayload getPerson)

        GET >=> route "/persons" >=> (handleRequest getPersons)

        POST >=> route "/persons"
             >=> bindJson<CreatePersonRequest> (handlePayload createPerson)

        PUT >=> route "/persons"
            >=> bindJson<UpdateGeneralRequest> (handlePayload updateGeneral)

        DELETE >=> routef "/persons/%s" (handlePayload deletePerson)

    ]
