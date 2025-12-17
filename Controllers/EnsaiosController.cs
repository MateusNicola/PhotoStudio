using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;

namespace PhotoStudio.app.Controllers
{
    [ApiController]
    [Route("api/ensaios")]
    public class EnsaiosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnsaiosController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/ensaios
        [HttpPost]
        public async Task<IActionResult> Post(EnsaioModel model)
        {
            _context.Ensaios.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        // GET api/ensaios
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ensaios = await _context.Ensaios
                .OrderByDescending(e => e.Data)
                .ToListAsync();

            return Ok(ensaios);
        }

        // DELETE api/ensaios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ensaio = await _context.Ensaios.FindAsync(id);
            if (ensaio == null)
                return NotFound();

            _context.Ensaios.Remove(ensaio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
