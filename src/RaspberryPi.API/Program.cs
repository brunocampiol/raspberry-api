using Amazon;
using Amazon.DynamoDBv2;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RaspberryPi.API.Configuration;
using RaspberryPi.API.Database;
using RaspberryPi.API.Models.Options;
using RaspberryPi.API.Repositories;
using System.Text;

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
//builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIContagem", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Example: Bearer 12345abcdef",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var key = Encoding.ASCII.GetBytes(ApplicationConfiguration.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

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

app.UseAuthentication();
app.UseAuthorization();

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