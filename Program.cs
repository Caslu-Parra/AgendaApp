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
   app.UseSwaggerUI(c =>
   {
      c.DefaultModelsExpandDepth(-1); // Disable swagger schemas at bottom
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
   });
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

app.MapGroup("/consultas")
   .WithTags("Consultas")
   .MapConsultaRoutes();

app.Run();
