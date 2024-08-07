using AgendaApp.Data;
using AgendaApp.Models;
using AgendaApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Routes
{
    public static class ClienteEndPoints
    {
        public static void MapClienteEndPoints(this RouteGroupBuilder app)
        {
            app.MapGet("", async (AppDbContext db) =>
            {
                Cliente[] clientes = await db.Clientes.ToArrayAsync();
                return clientes.Any() ? Results.Ok(clientes) : Results.NoContent();
            })
            .Produces<Cliente[]>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

            app.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                Cliente? cliente = await db.Clientes.Where(e => e.Id == id)
                                                    .Include(e => e.Pets)
                                                    .FirstOrDefaultAsync();
                return cliente is not null ? Results.Ok(cliente) : Results.NotFound(cliente);

            })
            .Produces<Cliente>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/create", async (AppDbContext db, CreateClienteViewModel model) =>
            {
                if (!model.IsValid) return Results.BadRequest(model.Notifications);

                Cliente cliente = new()
                {
                    Nome = model.Nome,
                    CPF = model.CPF,
                    DtInclusao = DateTime.Now
                };
                try
                {
                    await db.Clientes.AddAsync(cliente);
                    await db.SaveChangesAsync();
                    return Results.Created();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            app.MapPatch("/update", async (AppDbContext db, UpdateClienteViewModel model) =>
            {
                if (!model.IsValid) return Results.BadRequest(model.Notifications);
                try
                {
                    Cliente? cliente = await db.Clientes.FindAsync(model.Id);
                    if (cliente is null) return Results.NotFound(model);

                    cliente.CPF = model.CPF;
                    cliente.Nome = model.Nome;

                    db.Clientes.Update(cliente);
                    await db.SaveChangesAsync();
                    return Results.Accepted();
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            })
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            app.MapDelete("delete/{id}", async (AppDbContext db, int id) =>
            {
                try
                {
                    Cliente? cliente = await db.Clientes.FindAsync(id);
                    if (cliente is null) return Results.NotFound();

                    db.Clientes.Remove(cliente);
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