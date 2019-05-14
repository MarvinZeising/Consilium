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
open Consilium
open Wiki
open WikiCollection
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.IdentityModel.Tokens
open System.Text
open Authentication

let routes =
    choose [
        ProjectController.routes
        UserController.routes
        WikiController.routes
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
       .UseAuthentication()
       //.UseResponseCaching()
       .UseGiraffe routes

let configureServices (services : IServiceCollection) =
    let mongo = MongoClient ("mongodb://root:abc123@ds123624.mlab.com:23624/consiliumdb")
    let db = mongo.GetDatabase "consiliumdb"

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(fun options ->
            options.TokenValidationParameters <- TokenValidationParameters(
                ValidateActor = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "consiliumapp.org",
                ValidAudience = "consiliumapp.org",
                IssuerSigningKey = SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)))
        ) |> ignore
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore
    services.AddProjectCollection(db.GetCollection<Project>("projects")) |> ignore
    services.AddUserCollection(db.GetCollection<User>("users")) |> ignore
    services.AddTabCollection(db.GetCollection<Tab>("wiki")) |> ignore

let configureLogging (builder : ILoggingBuilder) =
    let filter (l : LogLevel) = l.Equals LogLevel.Error

    builder.AddFilter(filter)
           .AddConsole()
           .AddDebug()
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
