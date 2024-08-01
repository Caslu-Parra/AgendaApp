using System.ComponentModel.DataAnnotations.Schema;

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
    }
}