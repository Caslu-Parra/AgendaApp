using Flunt.Notifications;
using Flunt.Validations;

namespace AgendaApp.ViewModels
{
    public abstract class MedicoViewModel : Notifiable<Notification>
    {
        public string CRM { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public bool IsValid
        {
            get
            {
                Consiste();
                return base.IsValid;
            }
        }
        protected virtual void Consiste() => AddNotifications(
            new Contract<Notification>().IsNotNullOrEmpty(CPF, "CPF", "Campo obrigatório")
                                        .IsGreaterOrEqualsThan(CPF, 11, "CPF", "Formato inválido - Deve conter 11 caracteres")
                                        .IsNotNullOrEmpty(CRM, "CRM", "Campo obrigatório")
                                        .IsGreaterOrEqualsThan(CRM, 8, "CRM", "Formato inválido - Deve conter 8 caracteres")
                                        .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
                                        .IsGreaterOrEqualsThan(Nome, 3, "Nome", "Deve conter pelo menos 3 caracteres"));
    }

    public class CreateMedicoViewModel : MedicoViewModel;

    public class UpdateMedicoViewModel : MedicoViewModel
    {
        public int Id { get; set; }
        protected override void Consiste()
        {
            base.Consiste();
            AddNotifications(new Contract<Notification>().IsGreaterThan(Id, 0, "Id", "Campo obrigatório"));
        }
    };
}