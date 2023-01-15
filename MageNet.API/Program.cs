using System.Text;
using System.Text.Json.Serialization;
using MageNet.Persistence;
using MageNetServices.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ctx =>
{
    ctx.AddSecurityDefinition("bearer", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "jwt",
        Scheme = "bearer"
    });

    ctx.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "bearer" },
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "jwt"
                },
                new List<string>()
            }
        }
    );
});
builder.Services.RegisterDbContext(builder.Configuration.GetConnectionString("default"));
builder.Services.RegisterAuthenticationServices(builder.Configuration);
builder.Services.RegisterValidationServices();
builder.Services.RegisterAttributeServices();
builder.Services.RegisterDataTransferObjects();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(op => op.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTSecurityKey:key")))
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "apiCompatibilityPolicy",
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();

    if (app.Environment.IsEnvironment("Docker"))
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
}

else
{
    app.UseHttpsRedirection();
}

app.UseCors("apiCompatibilityPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}