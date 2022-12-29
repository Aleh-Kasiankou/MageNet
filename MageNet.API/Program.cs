using MageNet.Persistence;
using MageNetServices.DI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDbContext(builder.Configuration.GetConnectionString("default"));
builder.Services.RegisterRepositoryServices();
builder.Services.RegisterValidationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Ubuntu"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using(var scope = app.Services.CreateScope())
    {
        var mageNetContext = scope.ServiceProvider.GetRequiredService<MageNetDbContext>();
        mageNetContext.Database.Migrate();
    }
}

else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();