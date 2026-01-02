using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;

namespace PhotoStudio.app.Services
{
    public class CategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaModel>> ListarTodasAsync()
        {
            return await _context.Set<CategoriaModel>()
                .OrderBy(c => c.Codigo)
                .ToListAsync();
        }

        public async Task<List<CategoriaModel>> ListarPorUsuarioAsync(int usuarioId)
        {
            return await _context.Categorias
                .Where(c => c.UsuarioId == usuarioId)
                .OrderBy(c => c.Codigo)
                .ToListAsync();
        }

        public async Task<CategoriaModel?> ObterPorIdAsync(int id)
        {
            return await _context.Set<CategoriaModel>().FindAsync(id);
        }

        public async Task CriarAsync(CategoriaModel categoria)
        {
            _context.Set<CategoriaModel>().Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(CategoriaModel categoria)
        {
            _context.Set<CategoriaModel>().Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var categoria = await ObterPorIdAsync(id);
            if (categoria != null)
            {
                _context.Set<CategoriaModel>().Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }
    }
}