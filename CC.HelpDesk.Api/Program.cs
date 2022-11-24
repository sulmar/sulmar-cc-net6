using Microsoft.AspNetCore.Builder;
using CC.HelpDesk.Domain;
using CC.HelpDesk.IRepositories;
using CC.HelpDesk.InMemoryRepositories;
using Serilog;
using Serilog.Formatting.Compact;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

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

// TODO: atak CSRF - jak to dokładnie działa, gdzie jest przechowywany token?
// TODO: w jaki sposób przechowywać sekretne klucze na produkcji? - dodać linki 
// TODO: w jaki sposób podzielić złozony projekt na domeny?
// TODO: wyświetlić listę uzytkowników w React.js
// TODO: middleware
// TODO: powiadamianie aplikacji webowej o zmianie statusu
// TODO: integracja z bazą danych SQL Server (implementacja DbRepositories)
// TODO: logowanie i role
// TODO: deployment (publish)


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

    logger.MinimumLevel.Debug();
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

// dotnet add package Swashbuckle.AspNetCore

// Rejestracja usług do generowania dokumentacji
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new() { Title = "CC HelpDesk API", Version="1.0" });
});

// Rejestracja usług do sprawdzania kondyncji Health Check
builder.Services.AddHealthChecks()
    .AddCheck("Ping", ()=> HealthCheckResult.Healthy())
    .AddCheck("Random", () =>
    {
        if (DateTime.Now.Minute % 2 == 0)
            return HealthCheckResult.Healthy();
        else
            return HealthCheckResult.Unhealthy();
    });

// dotnet add package AspNetCore.HealthChecks.UI
// dotnet add package AspNetCore.HealthChecks.UI.InMemory.Storage
builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(15);
    options.AddHealthCheckEndpoint("CC HelpDesk", "/hc");
}).AddInMemoryStorage();

// W przypadku błędu SSL
// dotnet dev-certs https --trust

// CORS
builder.Services.AddCors(options=>
    options.AddDefaultPolicy(policy=>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();

        // policy.WithOrigins("http://localhost:3000", "https:domain.com");
        // policy.WithMethods(new string[] { "GET"});
        // policy.AllowAnyHeader();
    })
    
  
);

var app = builder.Build();

app.UseCors();



// # MinimalApi

// REST API
// GET - pobierz
// POST - utwórz
// PUT - zamień
// PATCH - modyfikacja
// DELETE - usuń


app.MapGet("ping", () => Results.Ok("Pong"));



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
    // logger.LogDebug($"Get User By Id={id}");

    // dobra praktyka
    logger.LogDebug("Get user By Id={id}", id);

#if DEBUG

    System.Console.WriteLine("XXXX");

#endif


    var user = userRepository.Get(id);

    if (user == null)
        return Results.NotFound(); // 404 NotFound

    return Results.Ok(user);   // 200 OK
}).WithName("GetUserById")
.Produces<User>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

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

    if (!userRepository.Exists(id))
        return Results.NotFound();

    // TODO: dodać walidację 
    userRepository.Update(user);

    return Results.NoContent();
});

app.MapDelete("api/users/{id}", (int id, IUserRepository userRepository) =>
{
    if (!userRepository.Exists(id))
        return Results.NotFound();

    userRepository.Remove(id);

    return Results.Ok();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // http://localhost:5000/swagger

    app.MapHealthChecksUI(); // https://localhost:5001/healthchecks-ui
}

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    // dotnet add package AspNetCore.HealthChecks.UI.Client
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.Run();