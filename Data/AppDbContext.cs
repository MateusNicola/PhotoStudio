using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Models;

namespace PhotoStudio.app.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<EnsaioModel> Ensaios => Set<EnsaioModel>();

        public DbSet<ClienteModel> Clientes => Set<ClienteModel>();
    }
}
