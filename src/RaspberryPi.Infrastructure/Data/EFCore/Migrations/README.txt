https://www.youtube.com/watch?v=QzCSN9wN4JA

https://learn.microsoft.com/en-us/ef/core/cli/dotnet
https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli

Verify installation
$ dotnet ef

Install ef tools
$ dotnet tool install --global dotnet-ef
$ dotnet tool update --global dotnet-ef

Manual migration
$ dotnet ef migrations add InitialMigration -s ./src/RaspberryPi.API/RaspberryPi.API.csproj -p ./src/RaspberryPi.Infrastructure/RaspberryPi.Infrastructure.csproj -o ../RaspberryPi.Infrastructure/Data/EFCore/Migrations -c RaspberryDbContext
$ dotnet ef database update -s ./src/RaspberryPi.API/RaspberryPi.API.csproj -p ./src/RaspberryPi.Infrastructure/RaspberryPi.Infrastructure.csproj -c RaspberryDbContext

Create EF SQL migration script
$ dotnet ef migrations script -s ./src/RaspberryPi.API/RaspberryPi.API.csproj -p ./src/RaspberryPi.Infrastructure/RaspberryPi.Infrastructure.csproj -o ./ef-migration.sql