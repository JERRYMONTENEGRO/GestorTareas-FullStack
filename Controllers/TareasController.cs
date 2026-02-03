using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // IMPORTANTE: Para usar ToListAsync, FindAsync, etc.
using MiApiDotNet.Data;

namespace MiApiDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    // Variable para guardar la conexión a la DB
    private readonly AppDbContext _context;

    // CONSTRUCTOR: Aquí pedimos la Base de Datos (Inyección de Dependencias)
    // En Spring Boot esto sería parecido a usar @Autowired
    public TareasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/tareas
    [HttpGet]
    // Usamos 'async Task' para que el servidor pueda atender otras peticiones mientras la DB responde
    public async Task<IActionResult> ObtenerTodas()
    {
        // 'await' espera a que la DB termine sin bloquear el hilo
        var tareas = await _context.Tareas.ToListAsync();
        return Ok(tareas);
    }

    // GET: api/tareas/5
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);

        if (tarea == null)
        {
            return NotFound();
        }
        return Ok(tarea);
    }

    // POST: api/tareas
    [HttpPost]
    public async Task<IActionResult> CrearTarea(Tarea nuevaTarea)
    {
        // Agregamos a la "cola" de cambios
        _context.Tareas.Add(nuevaTarea);
        
        // 'SaveChanges' es el commit. Aquí es donde se genera el INSERT SQL real.
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerTodas), new { id = nuevaTarea.Id }, nuevaTarea);
    }
    // PUT: api/tareas/5
    // Este es el método que nos faltaba para el Checkbox
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarTarea(int id, Tarea tareaActualizada)
    {
        // 1. Verificación de seguridad
        if (id != tareaActualizada.Id) return BadRequest();

        // 2. Buscamos la tarea original
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea == null) return NotFound();

        // 3. Actualizamos los datos
        tarea.Nombre = tareaActualizada.Nombre;
        tarea.Completada = tareaActualizada.Completada; // <--- AQUÍ SE MARCA EL CHECK

        // 4. Guardamos en base de datos
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/tareas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea == null)
        {
            return NotFound();
        }

        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync(); // ¡No olvides guardar los cambios!

        return NoContent();
    }
}