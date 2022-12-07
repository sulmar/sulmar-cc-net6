using CC.HelpDesk.IRepositories;
using CC.HelpDesk.Domain;
using Org.BouncyCastle.Asn1.Ocsp;

namespace CC.HelpDesk.Api;


public class Calendar
{
    public DateTime[] Holidays { get; set; }
}

public static class Endpoints
{
    public static WebApplication MapHelpDesk(this  WebApplication app)
    {
        app.MapBasicEndpoints();
        app.MapUserEndpoints();
        app.MapFileEndpoints();
        app.MapTicketEndpoints();

        return app;
    }
}

public static class UserEndpoints 
{

    public static bool IsHoliday(this DateTime date, Calendar calendar)
    {
        return true;
    }

    public static WebApplication MapBasicEndpoints(this WebApplication app)
    {
        app.MapGet("ping", () => Results.Ok("Pong"));

        // ## Endpoints (punkty końcowe)
        app.MapGet("/", () => "Hello HelpDesk User!");

        return app;
    }

    // Metoda rozszerzająca (Extension Method)
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        // Wstrzykiwanie zaleznosci (Dependency Injection / IoC)
        app.MapGet("api/users", (IUserRepository userRepository) =>
        {
            var users = userRepository.GetAll();

            return users;

        });
        //.RequireAuthorization();

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

        return app;
    }
}
