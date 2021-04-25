# SpotifyStalker

## SpotifyStalker.ConsoleUi / SpotifyStalker.Data

These two projects are independent of the web application, SpotifyStalker2.

They can be used to query data from Spotify, saving it back to a SQL Server database for various purposes.

### Using SpotifyStalker.Data

There is a hard-coded connection string in SpotifyStalkerDbContext.cs. Obviously, that needs to be updated for the machine running this app.

There is a way to use a connection string in appsettings (see [here](https://stackoverflow.com/questions/56017952/how-to-set-connectionstring-from-appsettings-json-in-entity-framework-core)) and/or a user secrets file, however it's more work than I want to do right now, especially since I'm the only one likely to ever use this.

#### Deploying Database Updates

Use the .NET Core CLI to deploy database updates on your system.

Navigate to the root folder of SpotifyStalker.Data.

If the EF tools need to be installed, run this:

```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
```

To add a migration, run:

```
dotnet ef migrations add MigrationName
```

To apply the migrations, run:

```
dotnet ef database update
```

[Reference](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)

### Using SpotifyStalker.ConsoleUi

Once a database is created, set SpotifyStalker.ConsoleUi as your startup project, and run it. It will write data to your local SpotifyStalker database.



