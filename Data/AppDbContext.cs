using AgendaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().HasMany(e => e.Pets)
                                          .WithOne(e => e.Cliente)
                                          .HasForeignKey(e => e.ClienteId)
                                          .IsRequired();

            modelBuilder.Entity<Cliente>().HasIndex(e => e.CPF)
                                          .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=app.db;Cache=Shared");
    }
}