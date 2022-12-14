using CC.HelpDesk.Domain;
using CC.HelpDesk.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CC.HelpDesk.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication SeedUsers(this WebApplication app) 
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
            var users = scope.ServiceProvider.GetRequiredService<List<User>>();

            if (!context.Users.Any())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            return app;
        }

        /// <summary>
        /// Utworzenie bazy danych bez migracji
        /// </summary>
        public static WebApplication CreateDatabase<TContext>(this WebApplication app)
            where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();
           
            context.Database.EnsureCreated(); 

            return app;
        }

        /// <summary>
        /// Utworzenie lub aktualizacja bazy danych na podstawie migracji
        /// </summary>        
        public static WebApplication CreateOrAlterDatabase<TContext>(this WebApplication app)
           where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();
           
            context.Database.Migrate();

            return app;
        }
    }
}
