using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RaspberryPi.API.Configuration;
using RaspberryPi.API.Mapping;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Options;
using RaspberryPi.Application.Services;
using RaspberryPi.Domain.Commands;
using RaspberryPi.Domain.Core;
using RaspberryPi.Domain.Interfaces;
using RaspberryPi.Infrastructure.Data;
using RaspberryPi.Infrastructure.Data.Connection;
using RaspberryPi.Infrastructure.Data.Context;
using RaspberryPi.Infrastructure.Data.Repositories;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Options;
using RaspberryPi.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jsonOptions = AppJsonSerializerOptions.Default;
var config = builder.Configuration;
var connectionString = config.GetConnectionString("SqlLite");


builder.Services.Configure<JwtAppOptions>(config.GetSection(JwtAppOptions.SectionName));
builder.Services.Configure<AccuWeatherOptions>(config.GetSection(AccuWeatherOptions.SectionName));
builder.Services.Configure<ApiIpOptions>(config.GetSection(ApiIpOptions.SectionName));

builder.Services.AddHealthChecks()
                .AddSqlite(connectionString);
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = jsonOptions.PropertyNameCaseInsensitive;
                    options.JsonSerializerOptions.PropertyNamingPolicy = jsonOptions.PropertyNamingPolicy;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = jsonOptions.DictionaryKeyPolicy;
                });

// TODO use more specific CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<RaspberryContext>(options => options.UseSqlite(connectionString));

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "Please provide a valid token",
        Name = "Authorization",
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

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Jwt:Key"])),
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
    };
});

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssemblies(typeof(AspNetUserCommandHandler).Assembly);
    //cfg.RegisterServicesFromAssemblies(typeof(JwtAppOptions).Assembly);
});

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new SqliteConnectionFactory(connectionString));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<ISqlLiteKeyValueRepository, SqlLiteKeyValueRepository>();
builder.Services.AddSingleton<IJwtAppService, JwtAppService>();
builder.Services.AddSingleton<IRequestToDomainMapper,  RequestToDomainMapper>();

builder.Services.AddScoped<RaspberryContext>();
builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IAspNetUserRepository, AspNetUserRepository>();
builder.Services.AddScoped<IAspNetUserAppService, AspNetUserAppService>();
builder.Services.AddScoped<IAnonymousCommentRepository, AnonymousCommentRepository>();
builder.Services.AddScoped<IAccuWeatherService, AccuWeatherService>();
builder.Services.AddScoped<IGeoLocationService, GeoLocationService>();
builder.Services.AddScoped<IWeatherAppService, WeatherAppService>();
builder.Services.AddScoped<IHardwareAppService, HardwareAppService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<RaspberryContext>();
        db.Database.EnsureCreated();
    }
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll"); // TODO use more specific CORS policy

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
//databaseInitializer.Initialize();

app.Run();