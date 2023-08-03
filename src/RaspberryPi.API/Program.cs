using Amazon;
using Amazon.DynamoDBv2;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using RaspberryPi.API.Configuration;
using RaspberryPi.API.Contracts.Options;
using RaspberryPi.API.Repositories;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var jsonOptions = AppJsonSerializerOptions.Default;

//builder.Services.Configure<AwsOptions>(builder.Configuration.GetSection(AwsOptions.Aws));
var awsOptions = config.GetSection(AwsOptions.Aws)
                       .Get<AwsOptions>();

// Add services to the container.
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = jsonOptions.PropertyNameCaseInsensitive;
                    options.JsonSerializerOptions.PropertyNamingPolicy = jsonOptions.PropertyNamingPolicy;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = jsonOptions.DictionaryKeyPolicy;
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbClient = new AmazonDynamoDBClient(awsOptions.AccessKeyId, 
                                        awsOptions.SecretAccessKey, 
                                        RegionEndpoint.USEast1);

builder.Services.AddSingleton<IAmazonDynamoDB>(_ => dbClient);
builder.Services.AddSingleton<ICommentRepository, CommentRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();

AppJsonSerializerOptions.SetDefaultOptions();

app.Run();
