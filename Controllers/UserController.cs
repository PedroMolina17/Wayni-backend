using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wayni.Data;
using Wayni.Models;

namespace Wayni.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UserContext _context;

        public UsuarioController(UserContext context)
        {
            _context = context;
        }

   
        // GET: api/usuario/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }


        // GET: api/usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }


        // POST: api/usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
            {
                ModelState.AddModelError("Email", "El email ya está en uso.");
                return BadRequest(ModelState);
            }

            if (await _context.Usuarios.AnyAsync(u => u.PhoneNumber == usuario.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "El número de teléfono ya está en uso.");
                return BadRequest(ModelState);
            }

            _context.Usuarios.Add(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Verificar si el error es por duplicidad
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Duplicate entry"))
                {
                    ModelState.AddModelError("", "Error de duplicado en la base de datos.");
                    return Conflict(ModelState); 
                }
                return StatusCode(500, "No se pudo guardar el usuario. Intente nuevamente.");
            }

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT: api/usuario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest("ID mismatch.");
            }

            // Verificar si el usuario existe
            var existingUser = await _context.Usuarios.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Verificar si el email o número de teléfono ya están en uso por otro usuario
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email && u.Id != id))
            {
                ModelState.AddModelError("Email", "El email ya está en uso.");
                return BadRequest(ModelState);
            }

            if (await _context.Usuarios.AnyAsync(u => u.PhoneNumber == usuario.PhoneNumber && u.Id != id))
            {
                ModelState.AddModelError("PhoneNumber", "El número de teléfono ya está en uso.");
                return BadRequest(ModelState);
            }

            // Actualizar los campos del usuario existente con los nuevos valores
            existingUser.Name = usuario.Name;
            existingUser.Username = usuario.Username;
            existingUser.Email = usuario.Email;
            existingUser.PhoneNumber = usuario.PhoneNumber;
            existingUser.Password = usuario.Password;

            _context.Entry(existingUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/usuario/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
