# ITSM
## IT Service Managment Web Application

Web application for project management purposes made with ASP.NET CORE MVC, Entity Framework Core, and SQL Server.

## Requirements
- You need to have [.NET 6](https://dotnet.microsoft.com/en-us/download) installed on your computer

## Installing

1. Clone the repository.
2. Check if nuget packages are installed using .NET CLI or by using Visual Studio.
If not, restore them using Visual Studio GUI or by .NET CLI by running `dotnet restore` command.
3. Build the project.
4. Run the project by Visual Studio or by .NET CLI.

## How to Run

Using Visual Studio simply build all projects by pressing `CTRL + B` and launch project without debugging by pressing `CTRL + F5`.
Using .NET CLI go to ITSM directory with `ITSM.csproj` file, then run `dotnet build` and `dotnet run` commands.

## Running the tests
You can run the unit tests in Visual Studio using GUI. Just find the 'Tests' tab on the top toolbar.
With .NET CLI go to the main directory with `ITSM.sln` file and use `dotnet test` command.

## Test users you can log in

##### Admin user
    Email: admin@itsm.com
    Password: SuperSecret@1

##### Employee user
    Email: employee@itsm.com
    Password: SuperSecret@1

##### Standard user
    Email: standard@itsm.com
    Password: SuperSecret@1

## Login / Register
You can log in to an account / register using 'Login' or 'Register' button in the bottom left corner.
If you are trying to register please note that your password must meet the requirements.

## Built With

* [Asp.Net Core](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
* [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/)

## Author

Jakub Miłek