using Microsoft.EntityFrameworkCore;
using PhotoStudio.app.Components;
using PhotoStudio.app.Data;
using PhotoStudio.app.Services;
using PhotoStudio.app.Services.Clientes;
using PhotoStudio.app.Services.Ensaios;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpClient();

        builder.Services.AddScoped<ClienteService>();
        builder.Services.AddScoped<EnsaioService>();
        builder.Services.AddScoped<ToastService>();
        builder.Services.AddScoped<DashboardService>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}