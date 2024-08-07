using AgendaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Consulta> Consultas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Cliente.OnModelCreating(modelBuilder);
            Medico.OnModelCreating(modelBuilder);
            Consulta.OnModelCreating(modelBuilder);
            Atendimento.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=app.db;Cache=Shared")
                      .LogTo(Console.WriteLine, LogLevel.Information);

    }
}