# Consilium

A shift scheduling system, developed to replace and improve `jwmanagement.org`.
From the developer of `jwmanagement.org`

## Setup

- install vue-cli
- install dotnet-sdk
- install docker-compose
- clone the repo

## Development

### Database

The data is stored in a MongoDB.
For development/testing, you need to have a local MongoDB running at `mongodb://localhost:27017`.

Initially, you don't have to initialize anything.
The app will automatically create the database and all needed collections.

### Server

The server is written in F#.
If you aren't familiar with functional programming, it might be helpful to read a little about that.

You might want to use Visual Studio. Just open the .sln file :-)
There is also an extension for VS Code (ionide-fsharp) that turns it into a full-fledged IDE.
When using VS Code, though, you might need to do a `dotnet restore` before opening VS Code.

To start a dev server, open a terminal session.
Enter the `Server` folder and run `dotnet watch run`.
When started, the server runs at `http://localhost:5000/`.
The server will automatically restart whenever you change anything in the code.

### Client

The client is done with Vue and TypeScript.

VS Code seems to be perfectly suited for developing.
Just open the `Client` folder in VS Code.
There is an extension for VS Code (vetur) that adds code formatting and highlighting for Vue.

To start a dev client, open a terminal session.
Enter the `Client` folder and run `npm run serve`.
When started, open `http://localhost:8080/`.
The client will automatically reload whenever you change anything in the code.

## Testing

### Unit tests

Where it makes sense, try to write unit tests for the server business logic.
E.g., this is done for the validation functions.

You can run the unit tests via the test explorer in Visual Studio.
Using the command line, enter the `UnitTests` folder and run `dotnet test`.

### Integration tests

All the endpoints should be tested by an integration test.
They should not test complex behavior.
Instead, they should test for one success case and for one failure case.
For some endpoints, it might really be a good idea to write a few more tests for different scenarios.

To run the integration tests, you first have to start a local server.
To do so, enter the `Server` folder and run `dotnet run` or `dotnet watch run`.

Then run the integration tests via the test explorer in Visual Studio.
Or, alternatively, open a new terminal session, enter the `IntegrationTests` folder and run `dotnet test`.

You can also run the integration tests, separately.
For that, run `docker-compose -f .docker/integration.yml run integration`.
