// Services/AuthService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using PhotoStudio.app.Data;
using PhotoStudio.app.Models;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UsuarioModel?> ValidarUsuarioAsync(string identificacao, string senhaDigitada)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Identificacao.Trim() == identificacao.Trim() && u.Ativo == "S");

        if (usuario == null) return null;

        bool senhaValida = BCrypt.Net.BCrypt.EnhancedVerify(senhaDigitada, usuario.Senha);
        if (!senhaValida) return null;

        usuario.UltimoLogin = DateTime.Now;

        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task<bool> RegistrarUsuarioAsync(string nome, string identificacao, string senha)
    {
        var existe = await _context.Usuarios.AnyAsync(u => u.Identificacao == identificacao);
        if (existe) return false;

        var novoUsuario = new UsuarioModel
        {
            Nome = nome, // Salva o nome aqui
            Identificacao = identificacao,
            Senha = BCrypt.Net.BCrypt.EnhancedHashPassword(senha),
            Ativo = "S",
            CriadoEm = DateTime.Now,
            Perfil = "User"
        };

        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();
        return true;
    }

    // Retorna todos os usuários para a lista
    public async Task<List<UsuarioModel>> ListarTodosUsuariosAsync()
    {
        return await _context.Usuarios
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    // Exclui um usuário pelo ID
    public async Task<bool> ExcluirUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<UsuarioModel?> ObterPorIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<bool> AtualizarUsuarioAsync(UsuarioModel usuarioAtualizado, string? novaSenha)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioAtualizado.Id);
        if (usuario == null) return false;

        usuario.Nome = usuarioAtualizado.Nome;
        usuario.Identificacao = usuarioAtualizado.Identificacao;

        if (!string.IsNullOrWhiteSpace(novaSenha))
        {
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool Sucesso, string Mensagem)> AlterarSenhaPropriaAsync(string login, string? senhaAtual, string novaSenha)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Identificacao == login);

        if (usuario == null)
        {
            return (false, "Usuário não encontrado.");
        }

        senhaAtual = senhaAtual?.Trim();

        try
        {
            bool senhaValida = BCrypt.Net.BCrypt.EnhancedVerify(senhaAtual, usuario.Senha);

            if (!senhaValida)
            {
                return (false, "A senha atual está incorreta.");
            }
        }
        catch (Exception)
        {
            return (false, "Erro ao verificar senha. Formato inválido.");
        }

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        await _context.SaveChangesAsync();

        return (true, "Senha alterada com sucesso!");
    }
}