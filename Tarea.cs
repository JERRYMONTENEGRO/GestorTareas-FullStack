namespace MiApiDotNet;

public class Tarea
{
    public int Id { get; set; }
    public string? Nombre { get; set; } // El '?' significa que puede ser nulo (null safety)
    public bool Completada { get; set; }
}