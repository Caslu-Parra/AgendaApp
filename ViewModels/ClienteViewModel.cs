using Flunt.Notifications;
using Flunt.Validations;

namespace AgendaApp.ViewModels
{
    public class ClienteViewModel : Notifiable<Notification>
    {
        public string CPF { get; init; }
        public int PetId { get; init; }
        public string Nome { get; init; }
        public bool IsValid
        {
            get
            {
                Consiste();
                return base.IsValid;
            }
        }
        protected virtual void Consiste() { }
    }

    public class CreateClienteViewModel : ClienteViewModel
    {
        protected override void Consiste() => AddNotifications(new Contract<Notification>()
                                           .Requires()
                                           .IsNotNullOrEmpty(CPF, "CPF", "Campo obrigatório")
                                           .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
                                           .IsNotNull(PetId, "PetId", "Campo obrigatório"));
    }

    public class UpdateClienteViewModel : ClienteViewModel
    {
        public int Id { get; init; }
        public DateTime DtInclusao { get; init; }
        protected override void Consiste() => AddNotifications(new Contract<Notification>()
                                              .Requires()
                                              .IsNotNull(Id, "PetId", "Campo obrigatório")
                                              .IsNotNull(DtInclusao, "PetId", "Campo obrigatório")
                                              .IsLowerOrEqualsThan(DtInclusao, DateTime.Now, "DtInclusao", "Data deve menor ou igual ao dia de hoje")
                                              .IsNotNullOrEmpty(CPF, "CPF", "Campo obrigatório")
                                              .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
                                              .IsNotNull(PetId, "PetId", "Campo obrigatório"));

    };
}