using  Microsoft.AspNetCore.Builder;
using CC.HelpDesk.Domain;

var app = WebApplication.Create();

// # MinimalApi

// REST API
// GET - pobierz
// POST - utwórz
// PUT - zamień
// PATCH - modyfikacja
// DELETE - usuń





// ## Endpoints (punkty końcowe)
app.MapGet("/", () => "Hello HelpDesk User!");

app.MapGet("api/users", () => "Hello Users!");

// ## Zastosowanie reguł (constraint)
app.MapGet("api/users/{id:int:min(1)}", (int id)=> $"Hello user id = {id}");

app.MapGet("api/users/{name:alpha}", (string name)=>$"Hello user {name}");

app.MapPost("api/users", (User user) => $"Created {user.FirstName}");

app.Run();