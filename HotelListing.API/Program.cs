using HotelListing.API;
using HotelListing.API.Config;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Data.Configuration;
using HotelListing.API.Data.Models;
using HotelListing.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddControllers();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

UserRoleConfiguration.SeedIdentityAsync(app).Wait();

app.Run();
