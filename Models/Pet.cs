using System.ComponentModel.DataAnnotations;

namespace AgendaApp.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime dtInclusao { get; set; }
        public DateTime? dtUltVisita { get; set; }
    }
}