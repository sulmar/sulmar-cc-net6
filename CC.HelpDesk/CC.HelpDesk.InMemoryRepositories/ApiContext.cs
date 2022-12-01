using CC.HelpDesk.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
