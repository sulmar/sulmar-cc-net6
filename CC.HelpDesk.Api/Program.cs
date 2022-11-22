using  Microsoft.AspNetCore.Builder;
using CC.HelpDesk.Domain;
using CC.HelpDesk.IRepositories;
using CC.HelpDesk.InMemoryRepositories;

// var app = WebApplication.Create();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

/*
builder.Services.AddSingleton<IUserRepository, DbUserRepository>();
builder.Services.AddSingleton<SqlConection>(()=>new SqlConnection("connection));

*/

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
app.MapGet("api/users/{id:int:min(1)}", (int id, IUserRepository userRepository) => {
   
   var user = userRepository.Get(id);

   if (user == null)
      return Results.NotFound(); // 404 NotFound

   return Results.Ok(user);   // 200 OK
}).WithName("GetUserById");

app.MapGet("api/users/{name:alpha}", (string name, IUserRepository userRepository)=>
{
   var user = userRepository.GetByName(name);

    if (user == null)
      return Results.NotFound(); // 404 NotFound

   return Results.Ok(user);
});

app.MapPost("api/users", (User user, IUserRepository userRepository) => {
   
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