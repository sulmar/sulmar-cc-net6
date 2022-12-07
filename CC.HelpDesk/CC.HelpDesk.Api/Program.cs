using Serilog;
using Serilog.Formatting.Compact;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using CC.HelpDesk.Api;
using Microsoft.AspNetCore.Authentication.Negotiate;
using CC.HelpDesk.Api.Extensions;
using CC.HelpDesk.Infrastructure;
using CC.HelpDesk.Domain;

// var app = WebApplication.Create();

var builder = WebApplication.CreateBuilder(args);

string environmentName = builder.Environment.EnvironmentName;

builder.Configuration.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true); // default
builder.Configuration.AddXmlFile("appsettings.xml", optional: false);
builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true); // default
builder.Configuration.AddCommandLine(args); // dotnet run --nbpapi="domain.com" // default
builder.Configuration.AddEnvironmentVariables(c => c.Prefix = "CC"); // default
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

// TODO: powiadamianie aplikacji webowej o zmianie statusu
// TODO: dodać odnośnik z przykładem do Dapper
// TODO: bezpieczeństwo (uwierzytelnianie i autoryzacja)
// TODO: kompresja

var connectionString = builder.Configuration.GetConnectionString("HelpDeskConnectionString");

builder.Services.AddSingleton(sp => new List<User>
{
    new User("John", "Smith") { Email = "john.smith@domain.com", HashedPassword = "202cb962ac59075b964b07152d234b70" },
    new User("Kate", "Smith") { Email = "kate.smith@domain.com", HashedPassword = "202cb962ac59075b964b07152d234b70"  },
    new User( "Mark", "Spider") { Email = "mark.spider@domain.com", HashedPassword = "202cb962ac59075b964b07152d234b70" },
});

builder.Services.AddDbHelpDeskRepositories(connectionString);



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
    options.SwaggerDoc("v1", new() { Title = "CC HelpDesk API", Version = "1.0" });
});

// Rejestracja usług do sprawdzania kondyncji Health Check
builder.Services.AddHealthChecks()
    .AddCheck("Ping", () => HealthCheckResult.Healthy())
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
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();

        // policy.WithOrigins("http://localhost:3000", "https:domain.com");
        // policy.WithMethods(new string[] { "GET"});
        // policy.AllowAnyHeader();
    })


);

// dotnet add package Microsoft.AspNetCore.Authentication.Negotiate --version 6.0.0
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = 
// })
// .AddJwtBearer()
// builder.Services.AddAuthorization();


// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/windowsauth?view=aspnetcore-7.0&tabs=visual-studio
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Logger Middleware
app.Use(async (context, next) =>
{
    Console.WriteLine($"{DateTime.Now} {context.Request.Method} {context.Request.Path}");

    await next();

    Console.WriteLine($"{context.Response.StatusCode}");
});

// app.Use(async (context, next)=>
// {
//     if (context.Request.Headers.TryGetValue("X-Secret-Key", out var secretKey) 
//                         && secretKey == builder.Configuration["SecretKey"])
//         await next();
//     else
//         context.Response.StatusCode = StatusCodes.Status403Forbidden;

// });

// app.UseMiddleware<SecretKeyMiddleware>();
// app.UseSecretKey();

// app.UseAuthorization();

app.UseCors();



// # MinimalApi

// REST API
// GET - pobierz
// POST - utwórz
// PUT - zamień
// PATCH - modyfikacja
// DELETE - usuń

app.CreateDatabase<ApiDbContext>();
app.SeedUsers();

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