https://www.youtube.com/watch?v=QzCSN9wN4JA

https://learn.microsoft.com/en-us/ef/core/cli/dotnet
https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli

Verify installation
$ dotnet ef

Install ef tools
$ dotnet tool install --global dotnet-ef
$ dotnet tool update --global dotnet-ef

Manual migration
$ dotnet ef migrations add InitialMigration -p ./src/RaspberryPi.API/RaspberryPi.API.csproj -o ./Data/Migrations -c RaspberryContext
$ dotnet ef database update -p ./src/RaspberryPi.API/RaspberryPi.API.csproj -c RaspberryContext

Create EF SQL migration script
$ dotnet ef migrations script -p ./src/RaspberryPi.API/RaspberryPi.API.csproj -o ./ef-migration.sql