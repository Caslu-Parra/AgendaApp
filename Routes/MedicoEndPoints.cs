using AgendaApp.Data;
using AgendaApp.Models;
using AgendaApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Routes
{
    public static class MedicoEndPoints
    {
        public static void MapMedicoEndPoints(this RouteGroupBuilder app)
        {
            app.MapGet("", async (AppDbContext db) =>
            {
                var medicos = await db.Medicos.ToArrayAsync();
                return medicos.Any() ? Results.Ok(medicos) : Results.NoContent();
            })
            .Produces<Medico[]>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

            app.MapGet("{id}", async (int id, AppDbContext db) =>
            {
                var medico = await db.Medicos.FindAsync(id);
                return medico != null ? Results.Ok(medico) : Results.NotFound();
            })
            .Produces<Medico>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("create", async (CreateMedicoViewModel model, AppDbContext db) =>
            {
                if (!model.IsValid) return Results.BadRequest(model.Notifications);
                try
                {
                    var medico = new Medico
                    {
                        Nome = model.Nome,
                        CPF = model.CPF,
                        CRM = model.CRM,
                        DtInclusao = DateTime.Now
                    };
                    await db.Medicos.AddAsync(medico);
                    await db.SaveChangesAsync();

                    return Results.Created();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            app.MapPatch("update", async (UpdateMedicoViewModel model, AppDbContext db) =>
            {
                if (!model.IsValid) return Results.BadRequest(model.Notifications);
                try
                {
                    Medico? medico = await db.Medicos.FindAsync(model.Id);
                    if (medico is null) return Results.NotFound(medico);

                    medico.Nome = model.Nome;
                    medico.CRM = model.CRM;
                    medico.CPF = model.CPF;

                    db.Medicos.Update(medico);
                    await db.SaveChangesAsync();
                    return Results.Accepted();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            app.MapDelete("delete/{id}", async (int id, AppDbContext db) =>
            {
                try
                {
                    var medico = await db.Medicos.FindAsync(id);
                    if (medico == null) return Results.NotFound();

                    db.Medicos.Remove(medico);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}