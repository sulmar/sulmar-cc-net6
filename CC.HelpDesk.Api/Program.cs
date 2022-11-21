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
    var users = new List<User> 
    {
        new User { Id = 1, FirstName = "John", LastName = "Smith", Email = "john.smith@domain.com", CreatedOn = DateTime.Parse("2022-11-21 14:00") },
        new User { Id = 2, FirstName = "Kate", LastName = "Smith", Email = "kate.smith@domain.com" },
        new User { Id = 3, FirstName = "Mark", LastName = "Spider", Email = "mark.spider@domain.com" },
    };

    return users;
    
});

// ## Zastosowanie reguł (constraint)
app.MapGet("api/users/{id:int:min(1)}", (int id)=> $"Hello user id = {id}");

app.MapGet("api/users/{name:alpha}", (string name)=>$"Hello user {name}");

app.MapPost("api/users", (User user) => $"Created {user.FirstName}");

app.Run();