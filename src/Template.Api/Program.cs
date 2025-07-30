using AspNetCoreRateLimit;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Template.Config;
using Template.Infrastructure.Kafka;
using Template.Shared;
using Template.Shared.Extensions;


var builder = WebApplication.CreateBuilder(args);

//Cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration));

// Feature Flags
builder.Services.AddFeatureManagement();

// Caching
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});

// Rate Limiting
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// MediatR, Mapster, Polly, Hangfire, Quartz, OpenTelemetry, MiniProfiler
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddMapster();

var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(3);

builder.Services.AddHttpClient("emp", (sp, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7061");
})
.AddPolicyHandler(retryPolicy);


var featureFlags = builder.Configuration.GetSection("FeatureManagement").Get<ProjectFeatureFlags>() ?? new ProjectFeatureFlags();

if (featureFlags.UseOpenTelemetry)
{
    builder.Services.AddOpenTelemetry()
        .WithTracing(tracing => tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyServiceName"))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter());
}


if (featureFlags.UseElastic)
{
    builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(
        ctx.Configuration["ElasticSearchConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = string.Format(ctx.Configuration["ElasticSearchConfiguration:Index"], DateTime.UtcNow),
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
    }));
}

if (featureFlags.UseKafka)
{
    builder.Services.AddSingleton<IKafkaService, KafkaService>();
    builder.Services.AddHostedService<KafkaConsumerHostedService>();
}

builder.Services.AddSingleton(featureFlags);

// DI: Tüm modülleri ekle
builder.Services.AddProjectModules(builder.Configuration);

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Swagger + JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Template API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Serilog request logging
app.UseSerilogRequestLogging();

// Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseIpRateLimiting();

app.MapControllers();

// Örnek endpoint
app.MapGet("/api/ping", () => ApiResponse<string>.SuccessResponse("pong"));

var logger = Log.ForContext<Program>();
logger.Information("Elastic log testi başarılı! Zaman: {Tarih}", DateTime.Now);

app.Run();
