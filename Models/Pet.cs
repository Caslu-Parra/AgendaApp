using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgendaApp.Models
{
    [Table("Pets")]
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DtInclusao { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}