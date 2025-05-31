using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UrlShortener.API.Models;
using UrlShortener.API.Processor.Inteface;
using UrlShortener.Processor;
using UrlShortener.Repository;
using UrlShortener.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Get port from environment variable or default to 5116
var port = Environment.GetEnvironmentVariable("PORT") ?? "5116";

// Get MongoDB connection string from environment
var mongoConnectionString = Environment.GetEnvironmentVariable("MongoConnectionString");
if (string.IsNullOrWhiteSpace(mongoConnectionString))
{
    throw new Exception("MongoDB connection string is not configured in environment variables.");
}


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

builder.Services.AddOpenApi();

// register API versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// register controller
builder.Services.AddControllers();

// still use appsettings.json for DB and collection names only
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// replace connection string usage here with env var
builder.Services.AddSingleton<IMongoClient>(_ =>
    new MongoClient(mongoConnectionString));

// register MongoDB collection
builder.Services.AddScoped(s =>
{
    var settings = s.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = s.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<UrlMapping>(settings.CollectionName);
});

// register dependencies
builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<IUrlProcessor, UrlProcessor>();

// allow CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
}


app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();
