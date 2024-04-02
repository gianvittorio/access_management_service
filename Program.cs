using System.Text.Json;
using System.Text.Json.Serialization;
using AccessManagementService.Persistence.Postgres;
using AccessManagementService.Persistence.Repository;
using AccessManagementService.Persistence.Repository.Impl;
using AccessManagementService.Service.AccessManagement;
using AccessManagementService.Service.EmployerFacade;
using AccessManagementService.Service.EmployerFacade.Impl;
using AccessManagementService.Service.UserFacade;
using AccessManagementService.Service.UserFacade.Impl;
using AccessManagementService.Web.ExceptionHandlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add JsonSerializerOptions
builder.Services
    .AddSingleton<JsonSerializerOptions>(_ => new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    });

// Add repositories
builder.Services
    .AddSingleton<IAccessManagementRepository, AccessManagementRepository>();

// Add HttpClient
builder.Services
    .AddHttpClient<IAccessManagementService,
        AccessManagementService.Service.AccessManagement.Impl.AccessManagementService>();

// Add services
builder.Services
    .AddSingleton<IUserServiceFacade, UserServiceFacade>();
builder.Services
    .AddSingleton<IEmployerServiceFacade, EmployerServiceFacade>();
builder.Services
    .AddSingleton<IAccessManagementService, AccessManagementService.Service.AccessManagement.Impl.AccessManagementService>();

// Add controllers
builder.Services.AddControllers();

// Add exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add db context
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddDistributedMemoryCache();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers()
    .WithOpenApi();

app.UseExceptionHandler();

app.Run();
