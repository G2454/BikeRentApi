using Microsoft.EntityFrameworkCore;
using RentBikeApi.Auth;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Domain.Services;
using RentBikeApi.ErrorHandling;
using RentBikeApi.Infrastructure.Auth;
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

    // Users and authentication
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddSingleton<IPasswordHasher, IdentityPasswordHasher>();
    builder.Services.AddJwtAuthentication(builder.Configuration);

    // Error handling
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    // UseExceptionHandler() refuses to start without it, even though our handler writes the response itself
    builder.Services.AddProblemDetails();

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ErrorCodeLoggingFilter>();
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
        .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = InvalidModelStateResponse.Create);

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    await using (var scope = app.Services.CreateAsyncScope())
    {
        if (app.Configuration.GetValue<bool>("Database:MigrateOnStartup"))
            await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

        var adminUsername = app.Configuration["Auth:Admin:Username"];
        var adminPassword = app.Configuration["Auth:Admin:Password"];
        if (!string.IsNullOrWhiteSpace(adminUsername) && !string.IsNullOrWhiteSpace(adminPassword))
            await scope.ServiceProvider.GetRequiredService<IAuthService>().EnsureAdminAsync(adminUsername, adminPassword);
    }

    app.UseMiddleware<CorrelationIdMiddleware>();  
    app.UseSerilogRequestLogging();                  
    app.UseExceptionHandler();                      

    // API docs everywhere except Production (Development in Visual Studio, Docker in compose)
    if (!app.Environment.IsProduction())
    {
        app.MapOpenApi().AllowAnonymous();
        app.MapScalarApiReference().AllowAnonymous();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();   
    app.UseAuthorization();    

    app.MapControllers();

    app.Run();
}
catch (Exception exception) when (exception is not HostAbortedException)  
{
    Log.Fatal(exception, "The application failed to start");
}
finally
{
    Log.CloseAndFlush();  
}