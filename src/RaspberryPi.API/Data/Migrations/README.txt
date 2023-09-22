https://www.youtube.com/watch?v=QzCSN9wN4JA

https://learn.microsoft.com/en-us/ef/core/cli/dotnet
https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli

Install ef tools
$ dotnet tool install --global dotnet-ef
$ dotnet tool update --global dotnet-ef

Manual migration
$ dotnet ef migrations add InitialMigration -p ./src/RaspberryPi.API/RaspberryPi.API.csproj -o ./Data/Migrations -c BloggingContext
$ dotnet ef database update -p ./src/RaspberryPi.API/RaspberryPi.API.csproj -c BloggingContext