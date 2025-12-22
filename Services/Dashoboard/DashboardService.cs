using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;

namespace PhotoStudio.app.Services
{
    public class DashboardService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public DashboardService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var ensaios = await db.Ensaios
                .Include(e => e.Cliente)
                .ToListAsync();

            var clientes = await db.Clientes.ToListAsync();

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