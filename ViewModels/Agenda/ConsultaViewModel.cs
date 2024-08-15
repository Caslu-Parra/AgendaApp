using Flunt.Notifications;
using Flunt.Validations;

namespace AgendaApp.ViewModels
{
    public abstract class ConsultaViewModel : Notifiable<Notification>
    {
        public DateOnly DtConsulta { get; set; }
        public int IdPet { get; set; }
        public int? IdAtendimento { get; set; }
        public new bool IsValid
        {
            get
            {
                Consiste();
                return base.IsValid;
            }
        }
        protected virtual void Consiste() => AddNotifications(
            new Contract<Notification>().IsNotNull(IdPet, "IdPet", "Campo obrigatório")
                                        .IsGreaterThan(IdPet, 0, "IdPet", "Deve ser maior que zero")
                                        .IsGreaterThan(IdAtendimento ?? 1, 0, "IdAtendimento", "Deve ser maior que zero")
                                        .IsNotNull(DtConsulta, "DtConsulta", "Campo obrigatório"));

    }
    public class CreateConsultaViewModel : ConsultaViewModel
    {
        protected override void Consiste()
        {
            base.Consiste();
            AddNotifications(new Contract<Notification>().IsGreaterOrEqualsThan(
                new DateTime(DtConsulta.Year, DtConsulta.Month, DtConsulta.Day), DateTime.Now.Date, "Id", "Campo obrigatório"));
        }
    }
    public class UpdateConsultaViewModel : ConsultaViewModel
    {
        public int Id { get; set; }
        protected override void Consiste()
        {
            base.Consiste();
            AddNotifications(new Contract<Notification>().IsGreaterOrEqualsThan(Id, 0, "Id", "Deve ser maior ou igual a zero"));
        }
    }
}