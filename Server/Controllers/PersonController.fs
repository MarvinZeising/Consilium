namespace Consilium

module PersonController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open PersonTypes
    open PersonHandlers
    open Controller

    let routes : HttpFunc -> HttpContext -> HttpFuncResult = choose [

        GET >=> route "/persons" >=> (handleRequest getPersons)

        POST >=> route "/persons"
             >=> bindJson<CreatePersonRequest> (handlePayload createPerson)

        //putRoute "/persons" (fun next context ->
            //task {
            //    let! input = context.BindJsonAsync<UpdateTopicRequest>()
            //    return! json (updateTopic input) next context
            //})

        //DELETE >=> routef "/persons/%s" deletePerson

    ]
