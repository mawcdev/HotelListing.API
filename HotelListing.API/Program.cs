using HotelListing.API;
using HotelListing.API.Config;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Data.Configuration;
using HotelListing.API.Data.Models;
using HotelListing.API.Middlewares;
using HotelListing.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentityCore<ApiUser>().
    AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListingApi")
    .AddEntityFrameworkStores<HotelListingDbContext>()
    .AddDefaultTokenProviders();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

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

builder.Services.AddControllers()
    .AddOData(options =>
    {
        options.Select().Filter().OrderBy();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
