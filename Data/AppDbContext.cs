using Microsoft.EntityFrameworkCore;
using MiApiDotNet; // Para reconocer tu clase Tarea

namespace MiApiDotNet.Data;

public class AppDbContext : DbContext
{
    // El constructor pasa la configuración al padre (base)
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Esto le dice a EF Core que cree una tabla llamada "Tareas" basada en tu clase Tarea
    public DbSet<Tarea> Tareas { get; set; }
}