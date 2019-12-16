namespace Consilium

module Program =

    printfn "Initializing server..."

    open System
    open System.IO
    open Microsoft.AspNetCore.Builder
    open Microsoft.AspNetCore.Hosting
    open Microsoft.AspNetCore.Cors.Infrastructure
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.Logging
    open Giraffe
    open Microsoft.AspNetCore.Authentication.JwtBearer
    open Microsoft.IdentityModel.Tokens
    open System.Text
    open Authentication

    let routes =
        choose [
            Controller.routes
            ProjectController.routes
            UserController.routes
            KnowledgeBaseController.routes

            RequestErrors.NOT_FOUND "Not Found"
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
           .UseGiraffe routes

    let configureServices (services : IServiceCollection) =
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(fun options ->
                    options.TokenValidationParameters <- TokenValidationParameters(
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "consiliumapp.org",
                        ValidAudience = "consiliumapp.org",
                        IssuerSigningKey = SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)))) |> ignore
        services.AddCors() |> ignore
        services.AddGiraffe() |> ignore

    let configureLogging (builder : ILoggingBuilder) =
        let filter (l : LogLevel) = l.Equals LogLevel.Error

        builder.AddFilter(filter)
               .AddConsole()
               .AddDebug()
               |> ignore

    printfn "Initialized!"

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
