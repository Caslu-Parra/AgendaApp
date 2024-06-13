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
            });

            app.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                Cliente? cliente = await db.Clientes.Where(e => e.Id == id)
                                                    .Include(e => e.Pets)
                                                    .FirstOrDefaultAsync();
                return cliente is not null ? Results.Ok(cliente) : Results.NotFound(cliente);

            });

            app.MapPost("/create", async (AppDbContext db, CreateClienteViewModel model) =>
            {
                if (!model.IsValid)
                    return Results.BadRequest(model);

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
                    return Results.Created(string.Empty, cliente);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message, ex.InnerException.Message, 500);
                }
            });

            app.MapPut("/update", async (AppDbContext db, UpdateClienteViewModel model) =>
            {
                if (!model.IsValid) return Results.BadRequest(model);
                try
                {
                    Cliente? cliente = await db.Clientes.FindAsync(model.Id);
                    if (cliente is null) return Results.NotFound(model);

                    cliente.CPF = model.CPF;
                    cliente.Nome = model.Nome;

                    db.Clientes.Update(cliente);
                    await db.SaveChangesAsync();
                    return Results.Ok(cliente);
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            });
        }
    }
}