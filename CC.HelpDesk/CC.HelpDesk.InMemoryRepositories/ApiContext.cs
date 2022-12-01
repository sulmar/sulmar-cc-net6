using CC.HelpDesk.Domain;
using CC.HelpDesk.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CC.HelpDesk.Infrastructure
{
    // dotnet add package Microsoft.EntityFrameworkCore --version 6.0.11
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
