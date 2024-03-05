using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Application.UseCases.Vehicle.Create;
using Car.Auction.Management.System.SqlServer.Configuration;
using Car.Auction.Management.System.SqlServer.Core;
using Car.Auction.Management.System.SqlServer.Mappings.Vehicle;
using Car.Auction.Management.System.SqlServer.QueryHandlers.Vehicle;
using Car.Auction.Management.System.Web.Core;
using Car.Auction.Management.System.Web.Mappings.Vehicle;
using Car.Auction.Management.System.Web.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var applicationSettings = builder.Configuration
    .GetSection("ApplicationSettings")
    .Get<ApplicationSettings>() ?? new ApplicationSettings();

builder.Services
    .AddDbContext<AppDbContext>(
        opt =>
            opt.UseSqlServer(applicationSettings.SqlServer.ConnectionString))
    .AddRepositories()
    .AddMappingConverters()
    .AddAutoMapper(opt =>
        opt.AddMaps(
            typeof(VehicleRequestMapping).Assembly,
            typeof(VehicleMapping).Assembly)
    )
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(
            typeof(Program).Assembly,
            typeof(CreateVehicleUseCase).Assembly,
            typeof(GetVehicleByIdQueryHandler).Assembly);
        cfg.AddOpenBehavior(typeof(ValidationMediatorDecorator<,>));
    })
    .AddFluentValidators()
    .AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();
app.UseExceptionMiddleware();
app.MapControllers();
app.Run();