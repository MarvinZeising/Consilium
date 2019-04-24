FROM microsoft/dotnet:2.2-sdk AS server
WORKDIR /src
COPY ./Server .
RUN dotnet publish "Server.fsproj" -c Release -o /app

FROM node:10 AS client
WORKDIR /src
COPY ./Client .
RUN npm install --no-optional
RUN ./node_modules/.bin/vue-cli-service build --dest /app

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=server /app .
COPY --from=client /app ./wwwroot
ENTRYPOINT ["dotnet", "Server.dll"]
