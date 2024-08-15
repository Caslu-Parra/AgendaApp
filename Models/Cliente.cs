using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string CPF { get; set; }
        public DateTime DtInclusao { get; set; }
        public string Nome { get; set; }
        public ICollection<Pet> Pets { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().HasMany(e => e.Pets)
                                          .WithOne(e => e.Cliente)
                                          .HasForeignKey(e => e.ClienteId)
                                          .IsRequired();
            // Index
            modelBuilder.Entity<Cliente>().HasIndex(e => e.CPF).IsUnique();
        }
    }
}