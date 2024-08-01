using AgendaApp.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

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
            #region Cliente
            //FK
            modelBuilder.Entity<Cliente>().HasMany(e => e.Pets)
                                          .WithOne(e => e.Cliente)
                                          .HasForeignKey(e => e.ClienteId)
                                          .IsRequired();
            // Index
            modelBuilder.Entity<Cliente>().HasIndex(e => e.CPF).IsUnique();
            #endregion

            #region Medico
            modelBuilder.Entity<Medico>().HasIndex(e => e.CRM).IsUnique();
            modelBuilder.Entity<Medico>().HasIndex(e => e.CPF).IsUnique();
            #endregion

            #region Consulta
            // Index
            modelBuilder.Entity<Consulta>().HasKey(e => new { e.Id, e.DtConsulta, e.IdPet });
            modelBuilder.Entity<Consulta>().HasIndex(e => e.IdAtendimento).IsUnique();
            // FK
            modelBuilder.Entity<Consulta>()
                        .HasOne(e => e.Pet)
                        .WithMany()
                        .HasForeignKey(e => e.IdPet)
                        .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Atendimento
            // FK 
            modelBuilder.Entity<Atendimento>()
                        .HasOne(e => e.Consulta)
                        .WithOne(e => e.Atendimento)
                        .HasPrincipalKey<Consulta>(e => e.Id)
                        .HasForeignKey<Atendimento>(e => e.IdConsulta)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired(false);

            modelBuilder.Entity<Atendimento>()
                        .HasOne(e => e.MedicoResp)
                        .WithMany()
                        .HasForeignKey(e => e.IdMedicoResp)
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired(true);
            #endregion
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=app.db;Cache=Shared");
    }
}