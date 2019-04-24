open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Projects
open Projects.ProjectCollection
open MongoDB.Driver
open Microsoft.Extensions.Logging
open System.IO

let routes =
    choose [
        ProjectController.routes
    ]

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse
    >=> ServerErrors.INTERNAL_ERROR ex.Message

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:80")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    app.UseGiraffeErrorHandler(errorHandler)
       .UseCors(configureCors)
       .UseDefaultFiles()
       .UseStaticFiles()
       //.UseAuthentication()
       //.UseResponseCaching()
       .UseGiraffe routes

let configureServices (services : IServiceCollection) =
    let mongo = MongoClient ("mongodb://root:abc123@ds123624.mlab.com:23624/consiliumdb")
    let db = mongo.GetDatabase "consiliumdb"

    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore
    services.AddProjectCollection(db.GetCollection<Project>("projects")) |> ignore

let configureLogging (builder : ILoggingBuilder) =
    let filter (l : LogLevel) = l.Equals LogLevel.Error

    builder.AddFilter(filter)
           .AddConsole()
           .AddDebug()
    // Add additional loggers if wanted...
    |> ignore

[<EntryPoint>]
let main _ =
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0
