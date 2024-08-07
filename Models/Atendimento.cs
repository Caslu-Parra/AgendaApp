using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Models
{
    [Table("Atendimentos")]
    public class Atendimento
    {
        public int Id { get; set; }
        public int IdConsulta { get; set; }
        public Consulta Consulta { get; set; }
        public int IdMedicoResp { get; set; }
        public Medico MedicoResp { get; set; }
        public string Anotacoes { get; set; }
        public DateTime DtInclusao { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}