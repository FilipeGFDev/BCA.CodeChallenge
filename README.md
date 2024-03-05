# BCA.CodeChallenge
This repository showss the solution for the code BCA challenge. This page provides insights into the development considerations, technologies used, design patterns, and other aspects of the solution.

## Technologies Used
- .NET 8
- Entity Framework Core (EF Core)
- Docker
- SQL Server

## Instructions to Use
1. Execute Docker compose command to create the SQLServer container:
   1. Run `docker-compose up -d` inside `Car.Auction.Management.System` directory;
2. Execute `dotnet ef` commands to create the database tables (inside `Car.Auction.Management.System` directory):
   1. Create migrations files: `dotnet ef migrations add InitialMigration --project Car.Auction.Management.System.SqlServer --startup-project Car.Auction.Management.System.Web`;
   2. Update database: `dotnet ef database update InitialMigration --project Car.Auction.Management.System.SqlServer --startup-project Car.Auction.Management.System.Web`;
   3. Note: After this 2 steps, is already possible see the tables created on the database;
3. Press Run/Debug on IDE;
4. **Optional**: Execute some requests via Postman, importing the collection `BCA-Car-Auction-Management-System.postman_collection.json`.

## Domain Model
<img src="./Car.Auction.Management.System\puml\aggs.png" alt="Model" width="400" height="400" />

- A Auction is exclusively associated with one Vehicle, and can receive multiple Bids;
- Vehicle utilizes inheritance to accommodate different types with distinct attributes;
- Common attributes include Id, Manufacturer, Model, Year, StartingBid, IsAvailable, and CreatedAt.

## Clean Architecture
Clean Architecture principles have been applied to create a modular and maintainable system. The architecture is organized into layers with distinct responsibilities, ensuring a separation of concerns and promoting flexibility for future changes:
- Entity Aggregates
- UseCases for the write requests
- UseCases Input Validators (using FluentValidation)
- Query handlers to acess the database
- Mediator pattern

### MediatR
Some of the application layers use MediatR to simplify the communication between them. This facilitates the decoupling of senders and receivers, promoting a more modular and maintainable design. This enables a dynamic routing mechanism where MediatR, based on the type of the request, intelligently directs the request to the corresponding `UseCaseHandler` or `QueryHandler`. 

### Decorator
The Decorator pattern was used to enhance behavior specifically for write requests, which typically involve HTTP methods like POST and PATCH. A dedicated decorator is responsible for validating input data before executing the corresponding use cases (`ValidationMediatorDecorator.cs`).

### Use Cases Input Validators
A robust validation mechanism is implemented for each UseCase, ensuring the integrity of aggregates before creation or modification. FluentValidations is utilized for this purpose, and a structured Error model (`Code` and `Message`) is introduced to handle and communicate validation failures to users with accuracy.

### Factory
In the Application project, a Factory pattern is employed to dynamically create entities within the Vehicle aggregate based on their specific types (`Hatchback`, `Sedan`, `Suv`, or `Truck`). At runtime, the factory determines the appropriate entity type to instantiate and initializes it with the correct information.

### Repository
In the project, the Repository pattern is employed to abstract the implementation details of aggregate repositories. The repository interface defines standard operations such as `GetById`, `GetAll`, `Create`, and `Update`, providing a unified and decoupled way to interact with aggregates. Notably, despite the `Vehicle` aggregate using inheritance, the `VehicleRepository` is designed to handle operations for various subtypes seamlessly.

### AutoMapper
AutoMapper has been used to the mapping process between different types consumed and returned by various layers of the application.

### Exception Middleware
A custom middleware has been implemented to handle exceptions, aiming to eliminate unpredictability in the application and ensure that meaningful messages are returned to consumers of the API.

## Tests
To ensure the robustness and correct functioning of the API, a comprehensive testing strategy (AAA) has been implemented, covering various aspects of the application.

### Unit tests
To ensure the reliability and correctness of individual components within the application, a suite of unit tests has been developed. For this, was used the Xunit alongside Fluent Assertions. 
Tests added:
- Controllers
- Mappings (Inputs, Responses, Requests and Proposals)
- Factories
- Helpers
- UseCases (Validators)

### Postman 
A basic Postman collection has been created to provide a convenient a set of requests for interacting with the API.