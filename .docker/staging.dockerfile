FROM node:10 AS client
WORKDIR /src
COPY ./Client .
RUN npm install --no-optional
RUN ./node_modules/.bin/vue-cli-service build --dest /app --mode staging

FROM microsoft/dotnet:2.2-sdk AS server
WORKDIR /src
RUN pwd
COPY ./Server ./Server
COPY ./Tests ./Tests
RUN dotnet test ./Tests/UnitTests/UnitTests.fsproj
RUN dotnet publish ./Server/Server.fsproj -c Release -o /app

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS final
WORKDIR /app
EXPOSE 80
COPY --from=server /app .
COPY --from=client /app ./wwwroot
ENTRYPOINT ["dotnet", "Server.dll"]
