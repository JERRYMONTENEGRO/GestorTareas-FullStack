using Microsoft.EntityFrameworkCore;
using MiApiDotNet.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. Configurar Base de Datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del entorno
// (Dejamos esto comentado o activo según prefieras, para ver Swagger)
// if (app.Environment.IsDevelopment()) 
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseCors("PermitirTodo"); // Activar CORS

app.UseAuthorization();

app.MapControllers();

// -----------------------------------------------------------
// EL NUEVO CÓDIGO VA AQUÍ:
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // ¡Esto crea la tabla Tareas si no existe!
}
// -----------------------------------------------------------

app.Run();