using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;
using Microsoft.AspNetCore.Components.Authorization; // Adicionado
using System.Security.Claims; // Adicionado

namespace PhotoStudio.app.Services.Ensaios
{
    public class EnsaioService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly AuthenticationStateProvider _authStateProvider;

        public EnsaioService(IDbContextFactory<AppDbContext> dbFactory, AuthenticationStateProvider authStateProvider)
        {
            _dbFactory = dbFactory;
            _authStateProvider = authStateProvider;
        }

        // Método auxiliar para pegar o ID do usuário logado
        private async Task<string> ObterUsuarioIdAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? throw new UnauthorizedAccessException("Usuário não identificado.");
        }

        public async Task<List<EnsaioModel>> ListarTodosAsync()
        {
            var userId = await ObterUsuarioIdAsync();
            using var db = _dbFactory.CreateDbContext();

            return await db.Ensaios
                .Where(e => e.UsuarioId == userId) // FILTRO POR USUÁRIO
                .Include(e => e.Cliente)
                .OrderByDescending(e => e.Data)
                .ToListAsync();
        }

        public async Task CriarAsync(EnsaioModel novoEnsaio)
        {
            var userId = await ObterUsuarioIdAsync();
            novoEnsaio.UsuarioId = userId; // VINCULA O NOVO ENSAIO AO USUÁRIO

            using var db = _dbFactory.CreateDbContext();
            db.Ensaios.Add(novoEnsaio);
            await db.SaveChangesAsync();
        }

        public async Task<EnsaioModel?> ObterPorIdAsync(int id)
        {
            var userId = await ObterUsuarioIdAsync();
            using var db = _dbFactory.CreateDbContext();

            return await db.Ensaios
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == userId); // GARANTE QUE É DELE
        }

        public async Task ExcluirAsync(int id)
        {
            var userId = await ObterUsuarioIdAsync();
            using var db = _dbFactory.CreateDbContext();

            // Só encontra se o ensaio pertencer ao usuário logado
            var ensaio = await db.Ensaios.FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == userId);

            if (ensaio != null)
            {
                db.Ensaios.Remove(ensaio);
                await db.SaveChangesAsync();
            }
        }

        public async Task AtualizarAsync(EnsaioModel ensaioEditado)
        {
            var userId = await ObterUsuarioIdAsync();

            // Segurança extra: garante que o UsuarioId não mude e pertença ao logado
            if (ensaioEditado.UsuarioId != userId)
                throw new UnauthorizedAccessException("Você não tem permissão para alterar este ensaio.");

            using var db = _dbFactory.CreateDbContext();
            db.Ensaios.Update(ensaioEditado);
            await db.SaveChangesAsync();
        }
    }
}