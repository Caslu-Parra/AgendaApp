using System.ComponentModel.DataAnnotations.Schema;

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
    }
}