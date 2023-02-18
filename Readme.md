# ITSM
### IT Service Managment Web Application

Web application for project management purposes made with ASP.NET CORE MVC, Entity Framework Core, and SQL Server.

### Requirements
- You need to have [.NET 6](https://dotnet.microsoft.com/en-us/download) installed on your computer

### Installing

1. Clone the repository.
2. Check if nuget packages are installed using .NET CLI or by using Visual Studio.
If not, restore them using Visual Studio GUI or by .NET CLI by running `dotnet restore` command.
3. Build the project.
4. Run the project by Visual Studio or by .NET CLI.

### How to Run

Using Visual Studio simply build all projects by pressing `CTRL + B` and launch project without debugging by pressing `CTRL + F5`.
Using .NET CLI go to ITSM directory with `ITSM.csproj` file, then run `dotnet build` and `dotnet run` commands.

### Running the tests
You can run the unit tests in Visual Studio using GUI. Just find the 'Tests' tab on the top toolbar.
With .NET CLI go to the main directory with `ITSM.sln` file and use `dotnet test` command.

# About application

### Use case
The ITSM application is created to facilitate project management.
The entire application is inspired by platforms such as Jira Service Desk and Azure DevOps.

### Home
If you are logged as employee or admin you can see the kanban board.
The use of the kanban board is to visualize the work items that have been added to the database.
You can drag work items and move them to the relevant states.

### Work Items
Each project can have multiple work items.
Work items are divided into three types: Task, Issue, and Epic.
Each type has its own use and its own characteristics.
For example Task has 'Remaning Work' field which shows how much work is left to complete a task, and Issue has 'Effort' field which shows on a scale of 1 to 10 how much effort it takes to solve this problem.
Note that you can only move work items by one position.

### Projects
Every project have it's description and work items assigned to it.
If we delete project, all of the assigned work items will be also deleted.

### States
Each state represents what is currently going on with the work item that it's describing.

### Tools
If you are logged as an admin you can see special 'Tools' tab.
In this tab you have two options:
You can add new job positions to your company, so if you will register new employee he will get appropriate position.
You can register the new employee that will get emlpoyee role will get roles that allows to managing work items, and projects.

### Test users you can log in

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
* [SQLite](https://www.sqlite.org/index.html)

## Author

Jakub Miłek