using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flunt.Notifications;
using Flunt.Validations;

namespace AgendaApp.ViewModels
{
    public abstract class AtendimentoViewModel : Notifiable<Notification>
    {
        public int IdConsulta { get; set; }
        public int IdMedicoResp { get; set; }
        public string Anotacoes { get; set; }
        public new bool IsValid
        {
            get
            {
                Consiste();
                return base.IsValid;
            }
        }
        protected virtual void Consiste() => AddNotifications(new Contract<Notification>()
                                            .IsGreaterOrEqualsThan(IdConsulta, 0, "IdConsulta", "Deve ser maior ou igual a zero")
                                            .IsGreaterOrEqualsThan(IdMedicoResp, 0, "IdMedicoResp", "Deve ser maior ou igual a zero")
                                            .IsNotNullOrEmpty(Anotacoes, "Anotacoes", "Campo obrigatório"));
    }

    public class CreateAtendimentoViewModel : AtendimentoViewModel;
    public class UpdateAtendimentoViewModel : AtendimentoViewModel
    {
        public int Id { get; set; }
        protected override void Consiste()
        {
            base.Consiste();
            AddNotifications(new Contract<Notification>().IsGreaterOrEqualsThan(Id, 0, "Id", "Deve ser maior ou igual a zero"));
        }
    }
}