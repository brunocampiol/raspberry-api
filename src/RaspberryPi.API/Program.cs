using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Org.BouncyCastle.Ocsp;
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

const string _corsPolicyName = "corsPolicy";
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

var origins = builder.Configuration
                     .GetSection("Cors:AllowedOrigins")
                     .Get<string[]>() ??
                     throw new InvalidOperationException("Cors:AllowedOrigins configuration is missing.");

// TODO: Use Rate Limiting Middleware

builder.Services.AddCors(options =>
{
    options.AddPolicy(_corsPolicyName, p =>
    {
        if (origins!.Contains("*")) p.AllowAnyOrigin();
        else p.WithOrigins(origins);

        p.AllowAnyMethod()
         .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<RaspberryDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();

        // NOTE: the type is IDictionary<string, IOpenApiSecurityScheme>
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        // Add Bearer scheme
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
        };

        // Global requirement (no scopes for bearer)
        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer"),
                    new List<string>()
                }
            }
        ];

        // Important when using references
        document.SetReferenceHostDocument();

        return Task.CompletedTask;
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

var fh = new ForwardedHeadersOptions
{
    ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost,
    ForwardLimit = 1
};

fh.KnownProxies.Add(System.Net.IPAddress.Loopback);
fh.KnownProxies.Add(System.Net.IPAddress.IPv6Loopback);

//app.UseHttpsRedirection();
app.UseForwardedHeaders(fh);

app.Use((ctx, next) =>
{
    if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix) &&
        !string.IsNullOrWhiteSpace(prefix))
    {
        var p = prefix.ToString().Trim();
        if (!p.StartsWith('/')) p = "/" + p;
        if (p.Length > 1 && p.EndsWith('/')) p = p.TrimEnd('/');

        ctx.Request.PathBase = p;
    }

    return next();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(_corsPolicyName);



app.MapOpenApi();
//app.UseSwagger(x =>
//{
//    x.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
//    {
//        if (!httpRequest.Headers.ContainsKey("X-Forwarded-Host")) return;

//        var serverUrl = $"{httpRequest.Headers["X-Forwarded-Proto"]}://" +
//                        $"{httpRequest.Headers["X-Forwarded-Host"]}/" +
//                        $"{httpRequest.Headers["X-Forwarded-Prefix"]}";

//        swaggerDoc.Servers = new List<OpenApiServer>()
//        {
//            new OpenApiServer { Url = serverUrl }
//        };
//    });
//});
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
    options.SwaggerEndpoint("../openapi/v1.json", "API v1");
    options.EnableTryItOutByDefault();
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