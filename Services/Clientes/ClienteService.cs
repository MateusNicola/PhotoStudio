using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace PhotoStudio.app.Services.Clientes
{
    public class ClienteService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly AuthenticationStateProvider _authStateProvider;

        public ClienteService(IDbContextFactory<AppDbContext> dbFactory, AuthenticationStateProvider authStateProvider)
        {
            _dbFactory = dbFactory;
            _authStateProvider = authStateProvider;
        }

        // Método privado para recuperar o ID do usuário logado de forma segura
        private async Task<string> ObterUsuarioIdAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? throw new UnauthorizedAccessException("Usuário não autenticado.");
        }

        public async Task<List<ClienteModel>> ListarTodosAsync()
        {
            var userId = await ObterUsuarioIdAsync();
            using var db = _dbFactory.CreateDbContext();

            return await db.Clientes
                .AsNoTracking()
                .Where(c => c.UsuarioId == userId) // Filtra apenas os clientes do usuário
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<ClienteModel?> ObterPorIdAsync(int id)
        {
            var userId = await ObterUsuarioIdAsync();
            using var db = _dbFactory.CreateDbContext();

            // Garante que o cliente pertence ao usuário antes de retornar
            return await db.Clientes
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == userId);
        }

        public async Task CriarAsync(ClienteModel novoCliente)
        {
            var userId = await ObterUsuarioIdAsync();
            novoCliente.UsuarioId = userId; // Vincula o cliente ao fotógrafo logado

            using var db = _dbFactory.CreateDbContext();
            db.Clientes.Add(novoCliente);
            await db.SaveChangesAsync();
        }

        public async Task AtualizarAsync(ClienteModel clienteAtualizado)
        {
            var userId = await ObterUsuarioIdAsync();

            // Validação de segurança: impede que o UsuarioId seja alterado maliciosamente
            if (clienteAtualizado.UsuarioId != userId)
                throw new UnauthorizedAccessException("Permissão negada para atualizar este cliente.");

            using var db = _dbFactory.CreateDbContext();
            db.Clientes.Update(clienteAtualizado);
            await db.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var userId = await ObterUsuarioIdAsync();
            using var db = _dbFactory.CreateDbContext();

            // Busca apenas se o ID e o Dono coincidirem
            var cliente = await db.Clientes
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == userId);

            if (cliente != null)
            {
                db.Clientes.Remove(cliente);
                await db.SaveChangesAsync();
            }
        }
    }
}