using System.Text.Json.Serialization;
using AgendaApp.Data;
using AgendaApp.Models;
using AgendaApp.ViewModels;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

#region Pets

app.MapGet("/pets", async (AppDbContext db) =>
{
    Pet[] pets = await db.Pets.ToArrayAsync();
    return pets.Any() ? Results.Ok(pets) : Results.NoContent();
});

app.MapGet("/pets/{id}", async (int id, AppDbContext db) =>
{
    Pet? pet = await db.Pets.Where(e => e.Id == id)
                            .Include(e => e.Cliente)
                            .FirstOrDefaultAsync();
    return pet is not null ? Results.Ok(pet) : Results.NotFound(pet);
});

app.MapPost("/pets/create", async (AppDbContext db, CreatePetViewModel model) =>
{
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);
    else if (db.Pets.Any(t => t.Nome == model.Nome))
        return Results.Conflict("Nome já existente");

    Pet pet = new Pet
    {
        Nome = model.Nome,
        ClienteId = model.ClienteId,
        DtInclusao = DateTime.Now
    };
    try
    {
        await db.Pets.AddAsync(pet);
        await db.SaveChangesAsync();
        return Results.Created(string.Empty, pet);
    }
    catch (DbUpdateException ex) { return Results.Problem(ex.Message, ex.InnerException.Message, 500); }
});

app.MapPut("/pets/update", async (AppDbContext db, UpdatePetViewModel model) =>
{
    if (!model.IsValid) return Results.BadRequest(model.Notifications);
    else if (await db.Pets.FindAsync(model.Id) is Pet pet &&
        await db.Clientes.FindAsync(pet.ClienteId) is not null)
    {
        pet.Nome = model.Nome;
        pet.DtInclusao = model.DtInclusao;
        pet.ClienteId = model.ClienteId;

        db.Pets.Update(pet);
        await db.SaveChangesAsync();
        return Results.Ok(pet);
    }
    else return Results.NotFound(model);

});

#endregion

#region Cliente

app.MapGet("/clientes", async (AppDbContext db) =>
{
    Cliente[] clientes = await db.Clientes.ToArrayAsync();
    return clientes.Any() ? Results.Ok(clientes) : Results.NoContent();
});

app.MapGet("/clientes/{id}", async (AppDbContext db, int id) =>
{
    Cliente? cliente = await db.Clientes.Where(e => e.Id == id)
                                        .Include(e => e.Pets)
                                        .FirstOrDefaultAsync();
    return cliente is not null ? Results.Ok(cliente) : Results.NotFound(cliente);

});

app.MapPost("/clientes/create", async (AppDbContext db, CreateClienteViewModel model) =>
{
    if (!model.IsValid)
        return Results.BadRequest(model);
    else if (db.Clientes.Any(e => e.CPF == model.CPF))
        return Results.Conflict(model);

    Cliente cliente = new()
    {
        Nome = model.Nome,
        CPF = model.CPF,
        PetId = model.PetId,
        DtInclusao = DateTime.Now
    };
    await db.Clientes.AddAsync(cliente);
    await db.SaveChangesAsync();
    return Results.Created(string.Empty, cliente);
});

app.MapPut("/clientes/update", async (AppDbContext db, UpdateClienteViewModel model) =>
{
    if (!model.IsValid) return Results.BadRequest(model);
    else if (await db.Clientes.FindAsync(model.Id) is Cliente cliente &&
             await db.Pets.FindAsync(cliente.PetId) is not null)
    {
        cliente.CPF = model.CPF;
        cliente.Nome = model.Nome;
        cliente.DtInclusao = model.DtInclusao;
        cliente.PetId = model.PetId;

        db.Clientes.Update(cliente);
        await db.SaveChangesAsync();
        return Results.Ok(cliente);
    }
    else return Results.NotFound(model);
});

#endregion

app.Run();
