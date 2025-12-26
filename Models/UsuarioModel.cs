using System.ComponentModel.DataAnnotations.Schema;

public class UsuarioModel
{
    public int Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    public string Identificacao { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    [Column("criado_em")] 
    public DateTime? CriadoEm { get; set; }

    [Column("ultimo_login")]
    public DateTime? UltimoLogin { get; set; }

    [Column("ativo")]
    public string Ativo { get; set; } = "S";

    [Column("alterado_por")]
    public string? AlteradoPor { get; set; }

    [Column("criado_por")]
    public string? CriadoPor { get; set; }

    [Column("alterado_em")]
    public DateTime? AlteradoEm { get; set; }

    public string Perfil { get; set; } = "User";
}