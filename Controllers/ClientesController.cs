using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/clientes
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var clientes = await _context.Clientes
            .OrderBy(c => c.Nome)
            .ToListAsync();

        return Ok(clientes);
    }

    // GET api/clientes/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    // POST api/clientes
    [HttpPost]
    public async Task<IActionResult> Post(ClienteModel model)
    {
        _context.Clientes.Add(model);
        await _context.SaveChangesAsync();

        return Ok(model);
    }

    // PUT api/clientes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ClienteModel model)
    {
        if (id != model.Id)
            return BadRequest();

        _context.Entry(model).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE api/clientes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
