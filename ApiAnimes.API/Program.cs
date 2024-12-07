using ApiAnimes.API.Middleware;
using ApiAnimes.Application.Mapping;
using ApiAnimes.Application.Services;
using ApiAnimes.Infra.Context;
using ApiAnimes.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.ExampleFilters(); 
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
        
        options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "Chave API necessária para acessar a API. Use o formato: X-API-KEY: {sua-chave}",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Name = "X-API-KEY",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Scheme = "ApiKeyScheme"
        });

        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                },
                new string[] {}
            }
        });
    });

var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    .Replace("{DB_USER}", user)
    .Replace("{DB_PASSWORD}", password);

Console.WriteLine($"Connection string: {connectionString}");

builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();
builder.Services.AddScoped<IAnimeService, AnimeService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<KeyMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    DbInitializer.InitDb(app);
}

app.Run();
