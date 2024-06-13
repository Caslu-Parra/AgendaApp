using System.Text.Json.Serialization;
using AgendaApp.Data;
using AgendaApp.Models;
using AgendaApp.Routes;
using AgendaApp.ViewModels;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

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
   .WithTags("clientes")
   .MapClienteEndPoints();

app.Run();
