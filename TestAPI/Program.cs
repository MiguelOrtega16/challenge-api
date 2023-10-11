using challenge_api_base.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ... otras configuraciones ...

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MyInMemoryDb"));

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IInformacionDeContactoService, InformacionDeContactoService>();

builder.Services.AddControllers();

// Configura Swashbuckle y añade Swagger
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" }); });

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

// Configura el middleware de Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge API v1");
    c.RoutePrefix = string.Empty; // Para acceder a Swagger en la raíz
});

// ... otras configuraciones de la tubería ...

app.Run();