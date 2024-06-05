using AgendaApp.Data;
using AgendaApp.Models;
using AgendaApp.ViewModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

#region Pets

app.MapGet("/pets", async (AppDbContext conexao) => Results.Ok(await conexao.Pets.ToArrayAsync()));

app.MapGet("/pets/{id}", async (int id, AppDbContext conexao) =>
{
    Pet? pet = await conexao.Pets.Where(x => x.Id == id)
                                 .Include(p => p.Cliente)
                                 .FirstOrDefaultAsync();

    return pet is not null ? Results.Ok(pet) : Results.NotFound(pet);
});

app.MapPost("/pets/create", async (AppDbContext conexao, PetViewModel model) =>
{
    model.Create();
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);
    else if (conexao.Pets.Any(t => t.Nome == model.Nome))
        return Results.Conflict("Nome já existente");

    Pet pet = new Pet
    {
        Nome = model.Nome,
        ClienteId = model.ClienteId,
        DtInclusao = DateTime.Now
    };

    await conexao.Pets.AddAsync(pet);
    await conexao.SaveChangesAsync();
    return Results.Created(string.Empty, pet);
});

app.MapPut("/pets/update", async (AppDbContext conexao, PetViewModel model) =>
{
    model.Update();
    if (!model.IsValid) return Results.BadRequest(model.Notifications);
    else if (await conexao.Pets.FindAsync(model.Id) is Pet pet &&
        await conexao.Clientes.FindAsync(pet.ClienteId) is not null)
    {
        pet.Nome = model.Nome;
        pet.DtInclusao = model.DtInclusao;
        pet.ClienteId = model.ClienteId;

        conexao.Pets.Update(pet);
        await conexao.SaveChangesAsync();
        return Results.Ok(pet);
    }
    else return Results.NotFound(model);

});

#endregion

#region Cliente



#endregion

app.Run();
