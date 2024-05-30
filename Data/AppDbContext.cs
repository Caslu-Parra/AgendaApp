using AgendaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Pet> Pets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=app.db;Cache=Shared");
    }
}