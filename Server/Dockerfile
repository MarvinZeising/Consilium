FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Server.fsproj", "./"]
RUN dotnet restore "./Server.fsproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Server.fsproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Server.fsproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Server.dll"]
