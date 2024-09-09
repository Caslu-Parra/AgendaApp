using AgendaApp.Routes;

namespace AgendaApp.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGroup("/pets")
               .WithTags("Pets")
               .MapPetEndPoints();

            app.MapGroup("/clientes")
               .WithTags("Clientes")
               .MapClienteEndPoints();

            app.MapGroup("/medicos")
               .WithTags("Medicos")
               .MapMedicoEndPoints();

            app.MapGroup("/consultas")
               .WithTags("Consultas")
               .MapConsultaRoutes();

            app.MapGroup("/atendimento")
               .WithTags("Atendimentos")
               .MapAtendimentoRoutes();
        }
    }
}
