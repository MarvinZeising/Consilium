open System
open System.IO
open MongoDB.Driver
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Giraffe
open Projects
open Projects.ProjectCollection
open Users
open Users.UserCollection

let routes =
    choose [
        ProjectController.routes
        UserController.routes
    ]

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse
    >=> ServerErrors.INTERNAL_ERROR ex.Message

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
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
    services.AddUserCollection(db.GetCollection<User>("users")) |> ignore

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
