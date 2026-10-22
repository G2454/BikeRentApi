using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using RentBikeApi.Infrastructure.Persistence;
using RentBikeApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Infrastructure.Repositories;
using RentBikeApi.Domain.Services;
using RentBikeApi.Infrastructure.Messaging;
using RentBikeApi.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
           .UseSnakeCaseNamingConvention());

builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();

//Messaging

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddHostedService<MotorcycleRegisteredConsumer>();

// Notifications

builder.Services.AddScoped<IMotorcycleNotificationRepository, MotorcycleNotificationRepository>();
builder.Services.AddScoped<IMotorcycleNotificationService, MotorcycleNotificationService>();


// Couriers
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<ICourierService, CourierService>();

// File Storage
builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));
builder.Services.AddSingleton<IFileStorage, LocalFileStorage>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = _ => new BadRequestObjectResult(new MessageResponse(MessageResponse.InvalidData)));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
