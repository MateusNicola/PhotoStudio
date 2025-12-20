using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;

namespace PhotoStudio.app.Services.Ensaios
{
    public class EnsaioService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public EnsaioService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<EnsaioModel>> ListarTodosAsync()
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Ensaios
                .Include(e => e.Cliente) // Importante para carregar os dados do cliente
                .OrderByDescending(e => e.Data)
                .ToListAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            using var db = _dbFactory.CreateDbContext();
            var ensaio = await db.Ensaios.FindAsync(id);
            if (ensaio != null)
            {
                db.Ensaios.Remove(ensaio);
                await db.SaveChangesAsync();
            }
        }

        public async Task CriarAsync(EnsaioModel novoEnsaio)
        {
            using var db = _dbFactory.CreateDbContext();
            db.Ensaios.Add(novoEnsaio);
            await db.SaveChangesAsync();
        }

        public async Task<EnsaioModel?> ObterPorIdAsync(int id)
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Ensaios
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AtualizarAsync(EnsaioModel ensaioEditado)
        {
            using var db = _dbFactory.CreateDbContext();
            db.Ensaios.Update(ensaioEditado);
            await db.SaveChangesAsync();
        }
    }
}