using Microsoft.AspNetCore.Builder;
using CC.HelpDesk.Domain;
using CC.HelpDesk.IRepositories;
using CC.HelpDesk.InMemoryRepositories;
using Serilog;
using Serilog.Formatting.Compact;

// var app = WebApplication.Create();

var builder = WebApplication.CreateBuilder(args);

string environmentName = builder.Environment.EnvironmentName;

builder.Configuration.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true); // default
builder.Configuration.AddXmlFile("appsettings.xml", optional: false); 
builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true); // default
builder.Configuration.AddCommandLine(args); // dotnet run --nbpapi="domain.com" // default
builder.Configuration.AddEnvironmentVariables(c=>c.Prefix="CC"); // default
builder.Configuration.AddUserSecrets<Program>();

// 
// Windows: cd %APPDATA%\Microsoft\UserSecrets\
// Linux/MacOS: cd ~/.microsoft/usersecrets

// do przeprowadzania testów integracyjnych
builder.Configuration.AddInMemoryCollection(
    new Dictionary<string, string>
    {
        { "NbpApi:Table", "C"},
        { "NbpApi:CurrencyCode", "USD"},
    }
);


string nbpApiUrl = builder.Configuration["NbpApi:Url"];

string azureSecretKey = builder.Configuration["AzureSecretKey"];

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

/*
builder.Services.AddSingleton<IUserRepository, DbUserRepository>();
builder.Services.AddSingleton<SqlConection>(()=>new SqlConnection("connection));

*/

// Log płaski
// builder.Logging.AddConsole();

// Log strukturalny
// builder.Logging.AddJsonConsole();

builder.Host.UseSerilog((context, logger) =>
{
    string seqHost = builder.Configuration["SeqHost"];

    logger.WriteTo.Console();
    logger.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
    logger.WriteTo.File(new CompactJsonFormatter(), "logs/log.json", rollingInterval: RollingInterval.Hour);
    logger.WriteTo.Seq(seqHost);

    // Enrich
    // dotnet add package Serilog.Enrichers.Environment
    logger.Enrich.WithEnvironmentName();
    logger.Enrich.WithMachineName();

    // dotnet add package Serilog.Sinks.Seq
});

// dotnet add package Serilog.AspNetCore




var app = builder.Build();

// # MinimalApi

// REST API
// GET - pobierz
// POST - utwórz
// PUT - zamień
// PATCH - modyfikacja
// DELETE - usuń





// ## Endpoints (punkty końcowe)
app.MapGet("/", () => "Hello HelpDesk User!");

// Wstrzykiwanie zaleznosci (Dependency Injection / IoC)
app.MapGet("api/users", (IUserRepository userRepository) =>
{

    var users = userRepository.GetAll();

    return users;

});

// ## Zastosowanie reguł (constraint)
app.MapGet("api/users/{id:int:min(1)}", (int id, IUserRepository userRepository, ILogger<Program> logger) =>
{
    if (id > 10)
    {
        throw new ApplicationException("Over limit");
    }

    // zła praktyka Interpolacja (interpolation)
    // logger.LogTrace($"Get User By Id={id}");

    // dobra praktyka
    logger.LogInformation("Get user By Id={id}", id);

#if DEBUG

    System.Console.WriteLine("XXXX");

#endif


    var user = userRepository.Get(id);

    if (user == null)
        return Results.NotFound(); // 404 NotFound

    return Results.Ok(user);   // 200 OK
}).WithName("GetUserById");

app.MapGet("api/users/{name:alpha}", (string name, IUserRepository userRepository) =>
{
    var user = userRepository.GetByName(name);

    if (user == null)
        return Results.NotFound(); // 404 NotFound

    return Results.Ok(user);
});

app.MapPost("api/users", (User user, IUserRepository userRepository) =>
{

    // TODO: dodać walidację 

    userRepository.Add(user);

    // Location: http://localhost:5000/api/users/4

    // zła praktyka
    // return Results.Created($"http://localhost:5000/api/users/{user.Id}", user);

    // dobra praktyka
    return Results.CreatedAtRoute("GetUserById", new { id = user.Id }, user);

});

app.MapPut("api/users/{id}", (int id, User user, IUserRepository userRepository) =>
{
    if (id != user.Id)
        return Results.BadRequest();

    // TODO: dodać walidację 
    userRepository.Update(user);

    return Results.NoContent();
});

app.MapDelete("api/users/{id}", (int id, IUserRepository userRepository) =>
{
    // TODO: dodac sprawdzenie czy uzytkownik istnieje

    userRepository.Remove(id);

    return Results.Ok();
});

app.Run();