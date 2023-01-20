using System.Text.Json.Serialization;
using MageNet.Persistence;
using MageNetServices.DI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDbContext(builder.Configuration.GetConnectionString("default"));
builder.Services.RegisterRepositoryServices();
builder.Services.RegisterValidationServices();
builder.Services.RegisterAttributeServices();
builder.Services.RegisterDataTransferObjects();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "apiCompatibilityPolicy",
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsEnvironment("Docker") || app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var mageNetContext = scope.ServiceProvider.GetRequiredService<MageNetDbContext>();
        if (mageNetContext.Database.IsRelational())
        {
            mageNetContext.Database.Migrate();
        }
    }
}


else
{
    app.UseHttpsRedirection();
}

app.UseCors("apiCompatibilityPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}