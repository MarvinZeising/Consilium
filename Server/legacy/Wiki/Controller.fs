namespace Wiki

open Giraffe
open Microsoft.AspNetCore.Http

module WikiController =

    let routes : HttpFunc -> HttpContext -> HttpFuncResult =
        choose [
            GET >=> route "/wiki" >=>
                fun next context ->
                    let find = context.GetService<TabFind>()
                    let tabs = find TabCriteria.All
                    json tabs next context

            GET >=> routef "/wiki/%s" (fun id ->
                fun next context ->
                    let find = context.GetService<TabFind>()
                    let tab = find( Id id )
                    json tab next context)
        ]
