using Microsoft.AspNetCore.Authentication.Cookies;
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
        builder.Services.AddSingleton<ToastService>();
        builder.Services.AddScoped<DashboardService>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Adicione o suporte a Cookies
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/acesso-negado";
                options.ExpireTimeSpan = TimeSpan.FromHours(8); // Sessão de 8 horas
            });


        // Antes do builder.Build()
        builder.Services.AddControllers();

        // Registre o AuthService e o HttpContextAccessor
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddCascadingAuthenticationState();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();
        app.MapControllers();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}