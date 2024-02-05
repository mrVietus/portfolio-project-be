# SuperCrawler Backend App

## Created with AzureFunctions

To run the application just start the application in VisualStudio
AzureFunction should start on [localhost:7180](http://localhost:7180). All endpoints should be visible after visiting `http://localhost:7180/api/swagger/ui`.

## Technologies Used

* [ASP.NET 8](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)

* [Azure SQL](https://learn.microsoft.com/en-us/azure/azure-sql/)

* [MediatR](https://github.com/jbogard/MediatR/)

* [Mapster](https://github.com/MapsterMapper/Mapster)

* [FluentValidation](https://fluentvalidation.net/)

* [ErrorOr](https://github.com/amantinband/error-or)

* [Html Agility Pack](https://html-agility-pack.net/)

* [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

* [NUnit](https://docs.nunit.org/index.html), [FluentAssertions](https://fluentassertions.com/), [NSubstitute](https://github.com/nsubstitute/NSubstitute/)

## Clean Architecture

![Clean Architecture Diagram](https://jasontaylor.dev/wp-content/uploads/2020/01/Figure-01-2.png)

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.


### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### Presentation

This layer is a ASP.NET Core MVC / Razor Pages application. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Program.cs* should reference Infrastructure.

### Summary

* **Independent of frameworks** – it does not require the existence of some tool or framework, clean, lightweight and easy to use

* **Independent of the database** – data-access concerns are cleanly separated

* **Independent of anything external** – in fact, Core (Domain + Application) is completely isolated from the outside world

* **Testable** – easy to test – Core (Domain + Application) has no dependencies on anything external, so writing automated tests is much easier

## Configuring connection to the frontend app and SQL

In the `FunctionHandler` project we have `local.settings.json` file.

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "Crawler:CacheItemsTimeSpanInDays": 1,
        "Crawler:CountOfTopWordsThatWillBeReturned": 15
    },
    "ConnectionStrings": {
        "CrawlerDatabase": "Server=localhost,1433; Database=*DB_NAME*; User Id=*DB_USERNAME*; Password=*SUPER_PASSWORD*; TrustServerCertificate=True;"
    },
    "Host": {
        "LocalHttpPort": 7180,
        "CORS": "*"
    }
}
```
