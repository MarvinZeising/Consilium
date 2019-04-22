open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Projects
open Projects.ProjectCollection
open MongoDB.Driver

let routes =
    choose [
        ProjectController.routes
    ]

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    app.UseCors(configureCors) |> ignore
    app.UseGiraffe routes

let configureServices (services : IServiceCollection) =
    let mongo = MongoClient ("mongodb://localhost:27017/")
    let db = mongo.GetDatabase "ConsiliumDb"

    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore
    services.AddProjectCollection(db.GetCollection<Project>("projects")) |> ignore

[<EntryPoint>]
let main _ =
    WebHostBuilder()
        .UseKestrel()
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .Build()
        .Run()
    0
