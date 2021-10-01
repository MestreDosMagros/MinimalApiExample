# Net 6 RC-1 Minimal Api Example using EF Core 6 preview

Example of a .Net 6 minimal API using EF Core 6 preview

## Managing migrations with ef core tools

#### To manage migrations with dotnet ef tools you will need to update the ef tool to the same rc version used in the project ef designer/tools

As the version used in this project is this: (can be checked on Infra.csproj/MinimalApi.csproj) 6.0.0-rc.1.21452.10, you will need to execute this command on your shell:

> dotnet tool update --global dotnet-ef --version 6.0.0-rc.1.21452.10

#### Creating a migration via ef tools cli

> dotnet ef migrations add <MIGRATION-NAME> -s "...\MinimalApi.csproj" -p "...\Infra.csproj"

#### Updating database via ef tools cli

> dotnet ef database update -s "...\MinimalApi.csproj" -p "...\Infra.csproj"

* Note that the ``-s`` parameter is your Startup project path and the ``-p`` parameter is your target project path (where the migration assembly is configured)

## Managing migrations with Nuget Package manager

To use the package manager tools, is the same as aways, select the infra project as target and then execute the commands

#### Creating migration

> Add-Migration <MIGRATION-NAME>

#### Updating database

> Update-Database
