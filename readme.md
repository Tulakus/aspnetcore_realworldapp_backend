# ![RealWorld Example App](logo.png)

> ### ASP.NET Core codebase containing real world examples (CRUD, auth, advanced patterns, etc) that adheres to the [RealWorld](https://github.com/gothinkster/realworld-example-apps) spec and API.

### [RealWorld](https://github.com/gothinkster/realworld)

This codebase was created to demonstrate a fully fledged fullstack application built with ASP.NET Core (with Feature orientation) including CRUD operations, authentication, routing, pagination, and more.

For more information on how to this works with other frontends/backends, head over to the [RealWorld](https://github.com/gothinkster/realworld) repo.

# How it works

This is using ASP.NET Core with:

- CQRS and [MediatR](https://github.com/jbogard/MediatR)
  - [Simplifying Development and Separating Concerns with MediatR](https://blogs.msdn.microsoft.com/cdndevs/2016/01/26/simplifying-development-and-separating-concerns-with-mediatr/)
  - [CQRS with MediatR and AutoMapper](https://lostechies.com/jimmybogard/2015/05/05/cqrs-with-mediatr-and-automapper/)
  - [Thin Controllers with CQRS and MediatR](https://codeopinion.com/thin-controllers-cqrs-mediatr/)
- [Fluent Validation](https://github.com/JeremySkinner/FluentValidation)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/) on SQLite or Microsoft SqlServer for demo purposes. Open to porting to other ORMs/DBs.
- JWT authentication using [ASP.NET Core JWT Bearer Authentication](https://github.com/aspnet/Security/tree/master/src/Microsoft.AspNetCore.Authentication.JwtBearer).

This basic architecture is based on this reference architecture: [https://github.com/jbogard/ContosoUniversityCore](https://github.com/jbogard/ContosoUniversityCore)

# Getting started

Install the .NET Core SDK and lots of documentation: [https://www.microsoft.com/net/download/core](https://www.microsoft.com/net/download/core)

Documentation for ASP.NET Core: [https://docs.microsoft.com/en-us/aspnet/core/](https://docs.microsoft.com/en-us/aspnet/core/)

## Local running

- appsettings.json - configure database type (sqlserver/sqlite) and accordingly set connection string
- run realworldapp from Visual Studio
- Postman [collection](https://github.com/gothinkster/realworld/blob/master/api/Conduit.postman_collection.json) to test API endpoint

## Notes

- tests coult be executed only against Microsoft SQl Server database - this is limitations of used database cleaner [Respawn](https://github.com/jbogard/Respawn)

## Todo

- generate api - Swagger
- AutoMapper
- building app - Cake
- port to ASP.NET core 3.1
