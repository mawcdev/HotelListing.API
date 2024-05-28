using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

app.UseAuthorization();

app.MapControllers();

app.Run();
