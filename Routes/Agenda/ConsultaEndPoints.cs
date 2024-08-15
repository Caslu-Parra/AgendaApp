using AgendaApp.Data;
using AgendaApp.Models;
using AgendaApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Routes
{
    public static class ConsultaEndPoints
    {
        public static void MapConsultaRoutes(this RouteGroupBuilder app)
        {
            app.MapGet("", async (AppDbContext db) =>
            {
                var consultas = await db.Consultas.ToArrayAsync();
                return consultas.Any() ? Results.Ok(consultas) : Results.NoContent();
            })
            .Produces<Consulta[]>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

            app.MapGet("{id}", async (int id, AppDbContext db) =>
            {
                var consulta = await db.Consultas.Include(fk => fk.Pet)
                                                 .Include(fk => fk.Atendimento).ThenInclude(fk => fk.MedicoResp)
                                                 .FirstOrDefaultAsync(c => c.Id == id);
                return consulta != null ? Results.Ok(consulta) : Results.NotFound();
            })
            .Produces<Consulta>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapPatch("update", async (AppDbContext db, UpdateConsultaViewModel model) =>
            {
                try
                {
                    if (!model.IsValid) return Results.BadRequest(model.Notifications);

                    Consulta? consulta = await db.Consultas.FirstOrDefaultAsync(c => c.Id == model.Id);
                    if (consulta is null) return Results.NotFound();

                    consulta.IdPet = model.IdPet;
                    consulta.DtConsulta = model.DtConsulta;
                    consulta.IdAtendimento = model.IdAtendimento;

                    db.Consultas.Update(consulta);
                    await db.SaveChangesAsync();
                    return Results.Accepted();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            app.MapPost("create", async (AppDbContext db, CreateConsultaViewModel model) =>
            {
                try
                {
                    if (!model.IsValid) return Results.BadRequest(model.Notifications);

                    var consulta = new Consulta
                    {
                        DtConsulta = model.DtConsulta,
                        DtInclusao = DateTime.Now,
                        IdPet = model.IdPet,
                        IdAtendimento = model.IdAtendimento
                    };
                    await db.Consultas.AddAsync(consulta);
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
                    Consulta? consulta = await db.Consultas.SingleOrDefaultAsync(c => c.Id == id);
                    if (consulta != null)
                    {
                        db.Consultas.Remove(consulta);
                        await db.SaveChangesAsync();
                        return Results.NoContent();
                    }
                    else return Results.NotFound();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}