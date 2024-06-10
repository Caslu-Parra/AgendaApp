using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}