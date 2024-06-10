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
        protected virtual void Consiste() { }
    }

    public class CreatePetViewModel : PetViewModel
    {
        protected override void Consiste() => AddNotifications(new Contract<Notification>()
                                              .Requires()
                                              .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
                                              .IsNotNull(ClienteId, "ClienteCPF", "Campo obrigatório"));
    }

    public class UpdatePetViewModel : PetViewModel
    {
        public int Id { get; set; }
        public DateTime DtInclusao { get; set; }
        protected override void Consiste() => AddNotifications(new Contract<Notification>()
                                              .Requires()
                                              .IsNotNull(Id, "Id", "Campo obrigatório")
                                              .IsNotNull(ClienteId, "ClienteCPF", "Campo obrigatório")
                                              .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
                                              .IsNull(DtInclusao, "DtInclusao", "Campo obrigatório"));
    }
}