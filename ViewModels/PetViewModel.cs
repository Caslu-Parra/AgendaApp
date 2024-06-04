using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgendaApp.Models;
using Flunt.Notifications;
using Flunt.Validations;

namespace AgendaApp.ViewModels
{
    public class PetViewModel : Notifiable<Notification>
    {
        public string Nome { get; set; }
        public int Id { get; set; }
        public DateTime dtInclusao { get; set; }
        public DateTime dtUltVisita { get; set; }

        public void Create()
        {
            AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório"));
        }

        public void Update()
        {
            AddNotifications(new Contract<Notification>()
           .Requires()
           .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
           .IsNotNull(Id, "Id", "Campo obrigatório")
           .IsGreaterOrEqualsThan(dtUltVisita, dtInclusao, "DtInclusao", "A última visita deve ser igual ou depois da inclusão"));
        }
    }
}