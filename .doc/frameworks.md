[Back to README](../README.md)

## Frameworks
Our frameworks are the building blocks that enable us to create robust, efficient, and maintainable software solutions. They have been carefully selected to complement our tech stack and address specific development challenges we face in our projects.

These frameworks enhance our development process by providing tried-and-tested solutions to common problems, allowing our team to focus on building unique features and business logic. Each framework has been chosen for its ability to integrate seamlessly with our tech stack, its community support, and its alignment with our development principles.

We use the following frameworks in this project:

### Backend
- **MediatR**: A behavioral design pattern that helps reduce chaotic dependencies between objects. It allows loose coupling by encapsulating object interaction.
  - Git: https://github.com/jbogard/MediatR
- **AutoMapper**: A convention-based object-object mapper that simplifies the process of mapping one object to another.
  - Git: https://github.com/AutoMapper/AutoMapper
- **FluentValidation**: A popular .NET library for building strongly-typed validation rules.
  - Git: https://github.com/FluentValidation/FluentValidation


### Messaging
- **Rebus**: A lean service bus implementation for .NET, providing a simple and flexible way to do messaging and queueing in .NET applications.
  - Git: https://github.com/rebus-org/Rebus
- **Rebus.RabbitMq**: RabbitMQ transport for Rebus.
  - Git: https://github.com/rebus-org/Rebus.RabbitMq
- **Rebus.ServiceProvider**: Microsoft.Extensions.DependencyInjection container adapter for Rebus.
  - Git: https://github.com/rebus-org/Rebus.ServiceProvider

### Database
- **Entity Framework Core**: A lightweight, extensible, and cross-platform version of Entity Framework, used for data access and object-relational mapping.
  - Git: https://github.com/dotnet/efcore
- **Npgsql.EntityFrameworkCore.PostgreSQL**: PostgreSQL provider for Entity Framework Core.
  - Git: https://github.com/npgsql/efcore.pg
- **MongoDB.Driver**: Official .NET driver for MongoDB.
  - Git: https://github.com/mongodb/mongo-csharp-driver

### Security & Authentication
- **BCrypt.Net-Next**: A .NET port of jBCrypt implemented in C# for password hashing.
  - Git: https://github.com/BcryptNet/bcrypt.net
- **Microsoft.AspNetCore.Authentication.JwtBearer**: JWT bearer authentication middleware for ASP.NET Core.
  - Git: https://github.com/dotnet/aspnetcore
- **System.IdentityModel.Tokens.Jwt**: JSON Web Token implementation for .NET.
  - Git: https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet

### API Documentation
- **Swashbuckle.AspNetCore**: Swagger tools for documenting APIs built on ASP.NET Core.
  - Git: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
- **Swashbuckle.AspNetCore.Filters**: Additional filters for Swashbuckle.AspNetCore.
  - Git: https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters

### Logging
- **Serilog**: A diagnostic logging library for .NET applications.
  - Git: https://github.com/serilog/serilog
- **Serilog.AspNetCore**: Serilog integration for ASP.NET Core.
  - Git: https://github.com/serilog/serilog-aspnetcore
- **Serilog.Exceptions**: Add exception details and custom properties to Serilog logs.
  - Git: https://github.com/RehanSaeed/Serilog.Exceptions

### Testing
- **xUnit**: A free, open source, community-focused unit testing tool for the .NET Framework.
  - Git: https://github.com/xunit/xunit
- **Bogus**: A simple and sane fake data generator for C#, F#, and VB.NET.
  - Git: https://github.com/bchavez/Bogus
- **NSubstitute**: A friendly substitute for .NET mocking libraries, used for creating test doubles in unit testing.
  - Git: https://github.com/nsubstitute/NSubstitute
- **FluentAssertions**: A set of .NET extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style unit test.
  - Git: https://github.com/fluentassertions/fluentassertions
- **Shouldly**: Should testing for .NET - the way assertions should be!
  - Git: https://github.com/shouldly/shouldly
- **Coverlet**: Cross platform code coverage framework for .NET.
  - Git: https://github.com/coverlet-coverage/coverlet
- **Roslynator**: A collection of 500+ analyzers, refactorings and fixes for C#, powered by Roslyn.
  - Git: https://github.com/JosefPihrt/Roslynator

### Utilities
- **Newtonsoft.Json**: Popular high-performance JSON framework for .NET.
  - Git: https://github.com/JamesNK/Newtonsoft.Json

<br>
<div style="display: flex; justify-content: space-between;">
  <a href="./tech-stack.md">Previous: Tech Stack</a>
  <a href="./general-api.md">Next: General API</a>
</div>