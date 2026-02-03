using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RaspberryPi.API.AutoMapper;
using RaspberryPi.API.Filters;
using RaspberryPi.API.HealthChecks;
using RaspberryPi.API.Helpers;
using RaspberryPi.API.Middlewares;
using RaspberryPi.API.Services;
using RaspberryPi.Application.Interfaces;
using RaspberryPi.Application.Models.Options;
using RaspberryPi.Application.Services;
using RaspberryPi.Domain.Helpers;
using RaspberryPi.Domain.Interfaces.Repositories;
using RaspberryPi.Domain.Interfaces.Services;
using RaspberryPi.Domain.Models.Options;
using RaspberryPi.Domain.Services;
using RaspberryPi.Infrastructure.Data.Context;
using RaspberryPi.Infrastructure.Data.Repositories;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Options;
using RaspberryPi.Infrastructure.Services;
using System.Text;

const string _corsPolicyName = "AllowAll";
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var connectionString = config.GetConnectionString("SqlLite")
    ?? throw new InvalidOperationException("Connection string 'SqlLite' is missing.");

builder.Services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));
builder.Services.Configure<WeatherOptions>(config.GetSection(WeatherOptions.SectionName));
builder.Services.Configure<FactOptions>(config.GetSection(FactOptions.SectionName));
builder.Services.Configure<GeoLocationOptions>(config.GetSection(GeoLocationOptions.SectionName));
builder.Services.Configure<IdentityAppOptions>(config.GetSection(IdentityAppOptions.SectionName));
builder.Services.Configure<EmailOptions>(config.GetSection(EmailOptions.SectionName));

builder.Services.AddAutoMapper(AutoMapperConfig.RegisterMappings());

builder.Services.AddHealthChecks()
                .AddSqlite(connectionString,
                           name: "sqlLite",
                           tags: ["database"])
                .AddCheck<WeatherHealthCheck>(
                            name: "weather",
                            tags: ["api"])
                .AddCheck<GeoLocationHealthCheck>(
                            name: "geolocation",
                            tags: ["api"])
                .AddCheck<FactHealthCheck>(
                            name: "fact",
                            tags: ["api"]);


builder.Services.AddControllers(options =>
                {
                    options.Filters.Add<RequestCounterFilter>();
                })
                .AddJsonOptions(opt =>
                {
                    var o = JsonDefaults.Options;

                    opt.JsonSerializerOptions.DictionaryKeyPolicy = o.DictionaryKeyPolicy;
                    opt.JsonSerializerOptions.PropertyNamingPolicy = o.PropertyNamingPolicy;
                    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = o.PropertyNameCaseInsensitive;
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = o.DefaultIgnoreCondition;
                    opt.JsonSerializerOptions.WriteIndented = o.WriteIndented;

                    foreach (var c in o.Converters)
                    {
                        opt.JsonSerializerOptions.Converters.Add(c);
                    }
                });

// TODO use more specific CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(_corsPolicyName, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<RaspberryDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
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

var jwtOptions = config.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException("Configuration section 'JwtOptions' is missing or invalid.");

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Key)),
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
    };
});

builder.Services.AddHttpClient();

//if (builder.Environment.IsProduction())
//{
//    // If adding, need to include in readme instructions to create the directory and set permissions
//    builder.Services.AddDataProtection()
//                    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys")) // Use a persistent, secure path in production
//                    .ProtectKeysWithAes(); // Optional: encrypt keys at rest
//}

builder.Services.AddMemoryCache(options =>
{
    options.TrackStatistics = true;
});

// App services
builder.Services.AddSingleton<RequestCounterService>();
builder.Services.AddSingleton<IMusicAppService, MusicAppService>();
builder.Services.AddScoped<IFeedbackAppService, FeedbackAppService>();
builder.Services.AddScoped<IWeatherAppService, WeatherAppService>();
builder.Services.AddScoped<IHardwareAppService, HardwareAppService>();
builder.Services.AddScoped<IGeoLocationAppService, GeoLocationAppService>();
builder.Services.AddScoped<IFactAppService, FactAppService>();
builder.Services.AddScoped<IEmailAppService, EmailAppService>();
builder.Services.AddScoped<IDatabaseAppService, DatabaseAppService>();
builder.Services.AddScoped<IWebsiteAppService, WebsiteAppService>();
// Domain services
builder.Services.AddSingleton<IBuzzerService, BuzzerService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IIdentityAppService, IdentityAppService>();
// Infra services
builder.Services.AddSingleton<IEmailInfraService, EmailInfraService>();
builder.Services.AddScoped<IWeatherInfraService, WeatherInfraService>();
builder.Services.AddScoped<IGeoLocationInfraService, GeoLocationInfraService>();
builder.Services.AddScoped<IFactInfraService, FactInfraService>();
// Repositories
builder.Services.AddScoped<RaspberryDbContext>();
builder.Services.AddScoped<IFeedbackMessageRepository, FeedbackMessageRepository>();
builder.Services.AddScoped<IGeoLocationRepository, GeoLocationRepository>();
builder.Services.AddScoped<IFactRepository, FactRepository>();
builder.Services.AddScoped<IEmailOutboxRepository, EmailOutboxRepository>();

var app = builder.Build();

app.UseMiddleware<ExceptionNotificationMiddleware>();

MethodTimeLogger.Logger = app.Logger;

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<RaspberryDbContext>();
    await db.Database.EnsureCreatedAsync();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(_corsPolicyName); // TODO use more specific CORS policy

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

app.MapHealthChecks("/ping", new HealthCheckOptions
{
    Predicate = _ => !_.Tags.Any(),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

await app.RunAsync();