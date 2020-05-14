# Consilium

A shift scheduling system, developed to replace and improve [JW Management](https://github.com/MarvinZeising/JWManagement) .

## Images

![alt text](https://github.com/MarvinZeising/Consilium/blob/development/ClientApp/public/1.png "Image 1")
![alt text](https://github.com/MarvinZeising/Consilium/blob/development/ClientApp/public/2.png "Image 2")
![alt text](https://github.com/MarvinZeising/Consilium/blob/development/ClientApp/public/3.png "Image 3")
![alt text](https://github.com/MarvinZeising/Consilium/blob/development/ClientApp/public/4.png "Image 4")

## Setup

* install vue-cli
* install dotnet-sdk
* clone the repo

## Development

### Database

The database is a Microsoft SQL DB.
Run the dbCreation.sql to initialize the structure.

### Server

The server is written in C#.

To start a dev server, open a terminal session.
Enter the `Server` folder and run `dotnet watch run` .
When started, the server runs at `http://localhost:5000/` and it already serves the client.
The server will automatically restart whenever you change anything in the code.

### Client

The client is done with Vue and TypeScript.

VS Code seems to be perfectly suited for developing.
Just open the `Client` folder in VS Code.
There is an extension for VS Code (vetur) that adds code formatting and highlighting for Vue.

To start a dev client only (without server), open a terminal session.
Enter the `Client` folder and run `npm run serve` .
When started, open `http://localhost:8080/` .
The client will automatically reload whenever you change anything in the code.
