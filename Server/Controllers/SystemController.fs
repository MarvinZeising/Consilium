namespace Consilium

module SystemController =

    open Giraffe
    open Microsoft.AspNetCore.Http
    open Controller

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [

            GET >=> route "/status" >=>
                fun next context ->
                    sendString (Ok "Server is up and running") next context

        ]
