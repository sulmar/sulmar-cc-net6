using Microsoft.AspNetCore.Builder;
using CC.HelpDesk.Domain;
using CC.HelpDesk.IRepositories;
using CC.HelpDesk.InMemoryRepositories;
using Serilog;
using Serilog.Formatting.Compact;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using CC.HelpDesk.Api;

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

// TODO: refaktoring endpointów
// TODO: w jaki sposób podzielić złozony projekt na domeny?
// TODO: middleware
// TODO: powiadamianie aplikacji webowej o zmianie statusu
// TODO: integracja z bazą danych SQL Server (implementacja DbRepositories)
// TODO: bezpieczeństwo (uwierzytelnianie i autoryzacja)
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



app.MapHelpDesk();


DateTime.Today.IsHoliday(new Calendar());


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