using System.Reflection.Metadata.Ecma335;
using AgendaApp.Data;
using AgendaApp.Models;
using AgendaApp.ViewModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.MapGet("/pets", async (AppDbContext conexao) => Results.Ok(await conexao.Pets.ToArrayAsync()));

app.MapGet("/pets/{id}", (int id, AppDbContext conexao) =>
{
    Pet? pet = conexao.Pets.Find(id);
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
        dtInclusao = DateTime.Now
    };

    await conexao.Pets.AddAsync(pet);
    await conexao.SaveChangesAsync();
    return Results.Created(string.Empty, pet);
});

app.MapPut("/pets/update", async (AppDbContext conexao, PetViewModel model) =>
{
    model.Update();
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    if (conexao.Pets.Find(model.Id) is Pet pet)
    {
        pet.Nome = model.Nome;
        pet.dtInclusao = model.dtInclusao;
        pet.dtUltVisita = model.dtUltVisita;

        conexao.Pets.Update(pet);
        await conexao.SaveChangesAsync();
        return Results.Ok(pet);
    }
    else return Results.NotFound(model);

});


app.Run();
