using Amazon;
using Amazon.DynamoDBv2;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using RaspberryPi.API.Configuration;
using RaspberryPi.API.Contracts.Options;
using RaspberryPi.API.Database;
using RaspberryPi.API.Repositories;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var jsonOptions = AppJsonSerializerOptions.Default;

// Add services to the container.
builder.Services.AddHealthChecks()
                .AddSqlite(config["Database:ConnectionString"]);
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = jsonOptions.PropertyNameCaseInsensitive;
                    options.JsonSerializerOptions.PropertyNamingPolicy = jsonOptions.PropertyNamingPolicy;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = jsonOptions.DictionaryKeyPolicy;
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var awsOptions = config.GetSection(AwsOptions.Aws)
                       .Get<AwsOptions>();

var dbClient = new AmazonDynamoDBClient(awsOptions.AccessKeyId, 
                                        awsOptions.SecretAccessKey,
                                        RegionEndpoint.USEast1);

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new SqliteConnectionFactory(config["Database:ConnectionString"]));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<ISqlLiteKeyValueRepository, SqlLiteKeyValueRepository>();
builder.Services.AddSingleton<IAmazonDynamoDB>(_ => dbClient);
builder.Services.AddSingleton<ICommentRepository, CommentRepository>();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
app.UseDeveloperExceptionPage();
app.UseSwagger(x =>
{
    x.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
    {
        if (!httpRequest.Headers.ContainsKey("X-Forwarded-Host")) return;

        var serverUrl = $"{httpRequest.Headers["X-Forwarded-Proto"]}://" +
                        $"{httpRequest.Headers["X-Forwarded-Host"]}/" +
                        $"{httpRequest.Headers["X-Forwarded-Prefix"]}";

        swaggerDoc.Servers = new List<OpenApiServer>()
        {
            new OpenApiServer { Url = serverUrl }
        };
    });
});
app.UseSwaggerUI(x =>
{
    x.EnableTryItOutByDefault();
});

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

AppJsonSerializerOptions.SetDefaultOptions();
//var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
////await databaseInitializer.InitializeAsync();

app.Run();