using Flunt.Notifications;
using Flunt.Validations;

namespace AgendaApp.ViewModels
{
    public class PetViewModel : Notifiable<Notification>
    {
        public string Nome { get; set; }
        public int ClienteId { get; set; }

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
                                              .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
                                              .IsGreaterOrEqualsThan(Nome, 3, "Nome", "Campo deve conter pelo menos 3 caracteres")
                                              .IsNotNull(ClienteId, "ClienteCPF", "Campo obrigatório"));
    }

    public class CreatePetViewModel : PetViewModel;

    public class UpdatePetViewModel : PetViewModel
    {
        public int Id { get; set; }
        protected override void Consiste() {
            base.Consiste();
            AddNotifications(new Contract<Notification>().Requires()
                                                         .IsNotNull(Id, "Id", "Campo obrigatório"));
        }
    }
}