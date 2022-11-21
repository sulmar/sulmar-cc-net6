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

app.MapGet("api/users", () => 
{
    
   throw new NotImplementedException();
    
});

// ## Zastosowanie reguł (constraint)
app.MapGet("api/users/{id:int:min(1)}", (int id) => new User(id, "John", "Smith"));

app.MapGet("api/users/{name:alpha}", (string name)=>$"Hello user {name}");

app.MapPost("api/users", (User user) => $"Created {user.FirstName}");

app.Run();