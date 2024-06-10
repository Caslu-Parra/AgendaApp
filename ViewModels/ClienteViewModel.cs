using Flunt.Notifications;
using Flunt.Validations;

namespace AgendaApp.ViewModels
{
    public class ClienteViewModel : Notifiable<Notification>
    {
        public string CPF { get; init; }
        public string Nome { get; init; }
        public bool IsValid
        {
            get
            {
                Consiste();
                return base.IsValid;
            }
        }
        protected virtual void Consiste() => AddNotifications(new Contract<Notification>()
                                           .Requires()
                                           .IsNotNullOrEmpty(CPF, "CPF", "Campo obrigatório")
                                           .IsGreaterOrEqualsThan(CPF, 11, "CPF", "Campo deve conter pelo menos 11 caracteres")
                                           .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
                                           .IsGreaterOrEqualsThan(Nome, 3, "Nome", "Campo deve conter pelo menos 3 caracteres"));
    }

    public class CreateClienteViewModel : ClienteViewModel;

    public class UpdateClienteViewModel : ClienteViewModel
    {
        public int Id { get; init; }
        protected override void Consiste()
        {
            base.Consiste();
            AddNotifications(new Contract<Notification>().IsNotNull(Id, "PetId", "Campo obrigatório"));
        }
    };
}