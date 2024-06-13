using AgendaApp.Data;
using AgendaApp.Models;
using AgendaApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Routes
{
    public static class PetEndPoints
    {
        public static void MapPetEndPoints(this RouteGroupBuilder app)
        {
            app.MapGet("", async (AppDbContext db) =>
            {
                Pet[] pets = await db.Pets.ToArrayAsync();
                return pets.Any() ? Results.Ok(pets) : Results.NoContent();
            }).Produces<Pet[]>().Produces(204);

            app.MapGet("{id}", async (int id, AppDbContext db) =>
            {
                Pet? pet = await db.Pets.Where(e => e.Id == id)
                                        .Include(e => e.Cliente)
                                        .FirstOrDefaultAsync();
                return pet is not null ? Results.Ok(pet) : Results.NotFound(pet);
            });

            app.MapPost("create", async (AppDbContext db, CreatePetViewModel model) =>
            {
                if (!model.IsValid) return Results.BadRequest(model.Notifications);
                try
                {
                    Pet pet = new Pet
                    {
                        Nome = model.Nome,
                        ClienteId = model.ClienteId,
                        DtInclusao = DateTime.Now
                    };
                    await db.Pets.AddAsync(pet);
                    await db.SaveChangesAsync();
                    return Results.Created(string.Empty, pet);
                }
                catch (DbUpdateException ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            });

            app.MapPut("update", async (AppDbContext db, UpdatePetViewModel model) =>
            {
                if (!model.IsValid) return Results.BadRequest(model.Notifications);
                try
                {
                    Pet? pet = await db.Pets.FindAsync(model.Id);
                    if (pet is null) return Results.NotFound(model);

                    pet.Nome = model.Nome;
                    pet.ClienteId = model.ClienteId;

                    db.Pets.Update(pet);
                    await db.SaveChangesAsync();
                    return Results.Ok(pet);
                }
                catch (Exception ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
            });
        }
    }
}