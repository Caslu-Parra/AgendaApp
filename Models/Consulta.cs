using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Models
{
    [Table("Consultas")]
    public class Consulta
    {
        public int Id { get; set; }
        public DateOnly DtConsulta { get; set; }
        public int IdPet { get; set; }
        public Pet Pet { get; set; }
        public int? IdAtendimento { get; set; }
        public Atendimento? Atendimento { get; set; }
        public DateTime DtInclusao { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consulta>().HasKey(e => new { e.Id, e.DtConsulta, e.IdPet });
            modelBuilder.Entity<Consulta>().HasIndex(e => e.IdAtendimento).IsUnique();
            // FK
            modelBuilder.Entity<Consulta>()
                        .HasOne(e => e.Pet)
                        .WithMany()
                        .HasForeignKey(e => e.IdPet)
                        .OnDelete(DeleteBehavior.NoAction);
        }
    }
}