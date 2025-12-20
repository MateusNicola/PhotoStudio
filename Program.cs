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

        // 1. Serviços básicos do Blazor
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpClient();

        // 2. Serviços da sua aplicação
        builder.Services.AddScoped<ClienteService>();
        builder.Services.AddScoped<EnsaioService>();
        builder.Services.AddScoped<ToastService>();

        // 3. Registro do Banco de Dados (APENAS ESTE É NECESSÁRIO)
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // O AddDbContextFactory já resolve tanto o IDbContextFactory quanto o AppDbContext
        builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        var app = builder.Build();

        // ... resto do código (Middleware, MapRazorComponents, etc)
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