using Microsoft.EntityFrameworkCore;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Domain.Services;
using RentBikeApi.ErrorHandling;
using RentBikeApi.Infrastructure.Messaging;
using RentBikeApi.Infrastructure.Persistence;
using RentBikeApi.Infrastructure.Repositories;
using RentBikeApi.Infrastructure.Storage;
using RentBikeApi.Middleware;
using Scalar.AspNetCore;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);


    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
               .UseSnakeCaseNamingConvention());

    builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
    builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();

    // Messaging
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

    // Rentals
    builder.Services.AddScoped<IRentalRepository, RentalRepository>();
    builder.Services.AddScoped<IRentalService, RentalService>();

    // Error handling
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ErrorCodeLoggingFilter>();
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
        .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = InvalidModelStateResponse.Create);

    builder.Services.AddOpenApi();

    var app = builder.Build();

    app.UseMiddleware<CorrelationIdMiddleware>();   
    app.UseSerilogRequestLogging();                 
    app.UseExceptionHandler();                      

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception) when (exception is not HostAbortedException)   // HostAbortedException = normal stop from "dotnet ef"
{
    Log.Fatal(exception, "The application failed to start");
}
finally
{
    Log.CloseAndFlush();  
}