using Audit.Core;
using HotelListing.API;
using HotelListing.API.Core.Config;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Middlewares;
using HotelListing.API.Core.Repository;
using HotelListing.API.Core.Users;
using HotelListing.API.Data;
using HotelListing.API.Data.Configuration;
using HotelListing.API.Data.Helpers;
using HotelListing.API.Data.Interceptors;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");

builder.Services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

builder.Services.AddDbContext<HotelListingDbContext>((sp, options) =>
{
    var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();
    options.UseSqlServer(connectionString).AddInterceptors(auditableInterceptor);
});

builder.Services.AddIdentityCore<ApiUser>().
    AddRoles<ApiRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListingApi")
    .AddEntityFrameworkStores<HotelListingDbContext>()
    .AddDefaultTokenProviders();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Listing API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = @$"JWT Authorization header using the {JwtBearerDefaults.AuthenticationScheme} scheme.
                        Enter '{JwtBearerDefaults.AuthenticationScheme}' [space] and then your token in the text input below.
                        Example: '{JwtBearerDefaults.AuthenticationScheme} 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="0auth2",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

/// <summary>
/// This shows the Cors Setup.
/// </summary>
/// <remarks>
/// <note>
/// This example demonstrates the adding of Cors Policy AllowAll.
/// </note>
/// </remarks>
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());
});


/// <summary>
/// This shows the Api Versioning set up
/// </summary>
/// <remarks>
/// <note>
/// This example demonstrates the addition of Api versioning on our Api
/// </note>
/// </remarks>
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver")
        );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
/// Api versioning

/// <summary>
/// This shows the Serilog Setup.
/// </summary>
/// <remarks>
/// <note>
/// This example demonstrates the configuration of serilog where to write where configuration to read from.
/// base on the host context.
/// </note>
/// </remarks>
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IUsersService, UsersService>();

AuthConfigurer.Configure(builder.Services, builder.Configuration);

/// <summary>
/// Caching Setup
/// </summary>
/// <remarks>
/// <note>
/// This example shows how to set up caching in our api
/// </note>
/// </remarks>
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024;
    options.UseCaseSensitivePaths = true;
});

builder.Services.AddHealthChecks()
    .AddCheck<CustomHealthCheck>("Custom Health Check", failureStatus: HealthStatus.Degraded, tags: ["custom"])
    .AddSqlServer(connectionString, tags: ["database"])
    .AddDbContextCheck<HotelListingDbContext>(tags: ["database"]);

builder.Services.AddControllers()
    .AddOData(options =>
    {
        options.Select().Filter().OrderBy();
    });

// Inject IPrincipal
builder.Services.AddHttpContextAccessor();

//Audit.Core.Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
//{
//    scope.SetCustomField("User", HttpContext.User.Identity.GetUserId());
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/healthcheck", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("custom"),
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
        [HealthStatus.Degraded] = StatusCodes.Status200OK
    },
    ResponseWriter = WriteResponse
});

app.MapHealthChecks("/databasehealthcheck", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("database"),
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
        [HealthStatus.Degraded] = StatusCodes.Status200OK
    },
    ResponseWriter = WriteResponse
});

static Task WriteResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json; charset=utf-8";

    var options = new JsonWriterOptions { Indented = true };

    using var memoryStream = new MemoryStream();
    using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
    {
        jsonWriter.WriteStartObject();
        jsonWriter.WriteString("status", report.Status.ToString());
        jsonWriter.WriteStartObject("results");

        foreach (var healthReportEntry in report.Entries)
        {
            jsonWriter.WriteStartObject(healthReportEntry.Key);
            jsonWriter.WriteString("status", healthReportEntry.Value.Status.ToString());
            jsonWriter.WriteString("description", healthReportEntry.Value.Description.ToString());
            jsonWriter.WriteStartObject("data");

            foreach (var item in healthReportEntry.Value.Data)
            {
                jsonWriter.WritePropertyName(item.Key);

                JsonSerializer.Serialize(jsonWriter, item.Value, item.Value?.GetType() ?? typeof(object));
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }
        jsonWriter.WriteEndObject();
        jsonWriter.WriteEndObject();
    }

    return context.Response.WriteAsync(
        Encoding.UTF8.GetString(memoryStream.ToArray()));
}

app.MapHealthChecks("/health");

app.UseMiddleware<ExceptionMiddleware>();

/// <summary>
/// This shows how to use Serilog Request Logging.
/// </summary>
/// <remarks>
/// <note>
/// This example demonstrates how to command serilog to log requests
/// </note>
/// </remarks>
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

/// <summary>
/// This shows how to use Cors on the App.
/// </summary>
/// <remarks>
/// <note>
/// This example demonstrates how to use Cors Policy AllowAll on the App.
/// </note>
/// </remarks>
app.UseCors("AllowAll");
if (bool.Parse(app.Configuration["Caching:IsEnabled"]))
{
    /// <summary>
    /// This shows how to use response caching.
    /// </summary>
    /// <remarks>
    /// <note>
    /// This example demonstrates how to tell our program to use response caching.
    /// </note>
    /// </remarks>
    app.UseResponseCaching();

    /// <summary>
    /// This shows how to use an inline middleware for response caching.
    /// </summary>
    /// <remarks>
    /// <note>
    /// This example demonstrates how to apply the response caching to every http context.
    /// </note>
    /// </remarks>
    app.Use(async (context, next) =>
    {
        //if (context.User.Identity?.IsAuthenticated != true)
        //{
        //    var result = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        //    if (result.Succeeded && result.Principal != null)
        //    {
        //        context.User = result.Principal;
        //    }
        //}

        context.Response.GetTypedHeaders().CacheControl =
            new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(double.Parse(app.Configuration["Caching:MaxAgeInSeconds"]))
            };
        string[] headerVary = ["Accept-Encoding"];
        context.Response.Headers[HeaderNames.Vary] = headerVary;
        await next();
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

UserRoleConfiguration.SeedIdentityAsync(app).Wait();

app.Run();


class CustomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        /* Custom checks. Logic..... etc. etc. */

        if (isHealthy)
        {
            return Task.FromResult(HealthCheckResult.Healthy("All systems are looking good."));
        }

        return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "System unlhealthy"));
    }
}