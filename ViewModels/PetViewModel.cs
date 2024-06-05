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
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public DateTime DtInclusao { get; set; }

        public void Create()
        {
            AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
            .IsNotNull(ClienteId, "ClienteCPF", "Campo obrigatório"));
        }

        public void Update()
        {
            AddNotifications(new Contract<Notification>()
           .Requires()
           .IsNotNull(Id, "Id", "Campo obrigatório")
           .IsNotNull(ClienteId, "ClienteCPF", "Campo obrigatório")
           .IsNotNullOrEmpty(Nome, "Nome", "Campo obrigatório")
           .IsNull(DtInclusao, "DtInclusao", "Campo obrigatório"));
        }
    }
}