using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace PhotoStudio.app.Services
{
    public class DashboardService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly AuthenticationStateProvider _authStateProvider;

        public DashboardService(IDbContextFactory<AppDbContext> dbFactory, AuthenticationStateProvider authStateProvider)
        {
            _dbFactory = dbFactory;
            _authStateProvider = authStateProvider;
        }

        private async Task<string> ObterUsuarioIdAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId ?? throw new UnauthorizedAccessException("Usuário não autenticado.");
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            var userId = await ObterUsuarioIdAsync();
            using var db = await _dbFactory.CreateDbContextAsync();

            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            // Importante: Filtramos direto no Banco de Dados (.Where) antes do ToListAsync
            var ensaios = await db.Ensaios
                .Where(e => e.UsuarioId == userId)
                .Include(e => e.Cliente)
                .ToListAsync();

            var clientes = await db.Clientes
                .Where(c => c.UsuarioId == userId)
                .ToListAsync();

            return new DashboardData
            {
                Ensaios = ensaios,
                Clientes = clientes,
                EnsaiosMes = ensaios.Count(e => e.Data >= inicioMes && e.Data <= fimMes),
                ClientesNovosMes = clientes.Count(c => c.CreatedAt >= inicioMes && c.CreatedAt <= fimMes),
                EnsaiosConcluidosMes = ensaios.Count(e => e.Data >= inicioMes && e.Data <= fimMes && e.Situacao == Situacao.Concluido),
                FaturamentoMes = ensaios.Where(e => e.Data >= inicioMes && e.Data <= fimMes && e.Valor.HasValue && e.Situacao == Situacao.Concluido)
                                        .Sum(e => e.Valor ?? 0)
            };
        }
    }

    public class DashboardData
    {
        public List<EnsaioModel> Ensaios { get; set; } = new();
        public List<ClienteModel> Clientes { get; set; } = new();
        public int EnsaiosMes { get; set; }
        public int ClientesNovosMes { get; set; }
        public decimal FaturamentoMes { get; set; }
        public int EnsaiosConcluidosMes { get; set; }
    }
}