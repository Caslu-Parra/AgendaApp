using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgendaApp.Models
{
    [Table("Medicos")]
    public class Medico
    {
        [Key]
        public int Id { get; set; }
        public string CRM { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DtInclusao { get; set; }
    }
}