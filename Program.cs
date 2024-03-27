using AccessManagementService.Persistence.Repository;
using AccessManagementService.Persistence.Repository.Impl;
using AccessManagementService.Service.AccessManagement;
using AccessManagementService.Service.EmployerFacade;
using AccessManagementService.Service.EmployerFacade.Impl;
using AccessManagementService.Service.UserFacade;
using AccessManagementService.Service.UserFacade.Impl;

var builder = WebApplication.CreateBuilder(args);


// Add repositories
builder.Services
    .AddSingleton<IAccessManagementRepository, AccessManagementRepository>();

// Add services
builder.Services
    .AddSingleton<IUserServiceFacade, UserServiceFacade>();
builder.Services
    .AddSingleton<IEmployerServiceFacade, EmployerServiceFacade>();
builder.Services
    .AddSingleton<IAccessManagementService, AccessManagementService.Service.AccessManagement.Impl.AccessManagementService>();

// Add controllers
builder.Services.AddControllers();

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

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

app.MapControllers();

// app.MapGet("/weatherforecast", () =>
//     {
//         var forecast = Enumerable.Range(1, 5).Select(index =>
//                 new WeatherForecast
//                 (
//                     DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                     Random.Shared.Next(-20, 55),
//                     summaries[Random.Shared.Next(summaries.Length)]
//                 ))
//             .ToArray();
//         return forecast;
//     })
//     .WithName("GetWeatherForecast")
//     .WithOpenApi();

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }