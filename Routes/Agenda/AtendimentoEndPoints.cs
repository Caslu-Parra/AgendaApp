using AgendaApp.Data;
using AgendaApp.Models;
using AgendaApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Routes
{
    public static class AtendimentoEndPoints
    {
        public static void MapAtendimentoRoutes(this RouteGroupBuilder app)
        {
            app.MapGet("", async (AppDbContext db) =>
            {
                var atendimentos = await db.Atendimentos.ToArrayAsync();
                return atendimentos.Any() ? Results.Ok(atendimentos) : Results.NoContent();
            })
            .Produces<Atendimento[]>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

            app.MapGet("{id}", async (int id, AppDbContext db) =>
            {
                var atendimento = await db.Atendimentos.Include(fk => fk.Consulta).ThenInclude(fk => fk.Pet)
                                                       .Include(fk => fk.MedicoResp)
                                                       .FirstOrDefaultAsync(c => c.Id == id);
                return atendimento is null ? Results.NotFound() : Results.Ok(atendimento);
            })
            .Produces<Atendimento>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapPatch("update", async (AppDbContext db, UpdateAtendimentoViewModel model) =>
            {
                try
                {
                    if (!model.IsValid) return Results.BadRequest(model.Notifications);

                    Atendimento? atendimento = await db.Atendimentos.FirstOrDefaultAsync(c => c.Id == model.Id);
                    if (atendimento is null) return Results.NotFound();
                    
                    atendimento.IdMedicoResp = model.IdMedicoResp;
                    atendimento.IdConsulta = model.IdConsulta;
                    atendimento.Anotacoes = model.Anotacoes;
                    db.Atendimentos.Update(atendimento);
                    await db.SaveChangesAsync();
                    return Results.Accepted();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            app.MapPost("create", async (AppDbContext db, CreateAtendimentoViewModel model) =>
            {
                try
                {
                    if (!model.IsValid) return Results.BadRequest(model.Notifications);

                    var atendimento = new Atendimento
                    {
                        DtInclusao = DateTime.Now,
                        Anotacoes = model.Anotacoes,
                        IdConsulta = model.IdConsulta,
                        IdMedicoResp = model.IdMedicoResp
                    };
                    await db.Atendimentos.AddAsync(atendimento);
                    await db.SaveChangesAsync();
                    return Results.Created();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            app.MapDelete("delete/{id}", async (AppDbContext db, int id) =>
            {
                try
                {
                    Atendimento? atendimento = await db.Atendimentos.FirstOrDefaultAsync(c => c.Id == id);
                    if (atendimento is null) return Results.NotFound();
                    
                    db.Atendimentos.Remove(atendimento);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}