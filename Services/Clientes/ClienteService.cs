using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;

namespace PhotoStudio.app.Services.Clientes
{
    public class ClienteService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public ClienteService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<ClienteModel>> ObterTodosAsync()
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Clientes
                           .OrderBy(c => c.Nome)
                           .ToListAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            using var db = _dbFactory.CreateDbContext();
            var cliente = await db.Clientes.FindAsync(id);
            if (cliente != null)
            {
                db.Clientes.Remove(cliente);
                await db.SaveChangesAsync();
            }
        }

        public async Task CriarAsync(ClienteModel novoCliente)
        {
            using var db = _dbFactory.CreateDbContext();
            db.Clientes.Add(novoCliente);
            await db.SaveChangesAsync();
        }

        public async Task<ClienteModel?> ObterPorIdAsync(int id)
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AtualizarAsync(ClienteModel clienteAtualizado)
        {
            using var db = _dbFactory.CreateDbContext();
            db.Clientes.Update(clienteAtualizado);
            await db.SaveChangesAsync();
        }

        public async Task<List<ClienteModel>> ListarTodosAsync()
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Clientes
                .AsNoTracking() // Melhora a performance para consultas de apenas leitura
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }
    }
}