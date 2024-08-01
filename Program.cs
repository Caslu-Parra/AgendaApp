using System.Text.Json.Serialization;
using AgendaApp.Data;
using AgendaApp.Routes;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>()
                .Configure<JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("/pets")
   .WithTags("Pets")
   .MapPetEndPoints();

app.MapGroup("/clientes")
   .WithTags("Clientes")
   .MapClienteEndPoints();

app.MapGroup("/medicos")
   .WithTags("Medicos")
   .MapMedicoEndPoints();

app.Run();
