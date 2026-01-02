using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoStudio.app.Models
{
    public class EnsaioModel
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }        
        public required DateTime Data { get; set; }
        public required string Local { get; set; }
        public decimal? Valor { get; set; }
        public required Situacao Situacao { get; set; }
        [Column("cliente_id")]
        public required int ClienteId { get; set; }
        public ClienteModel Cliente { get; set; } = null!;
        public string UsuarioId { get; set; }

        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual CategoriaModel? Categoria { get; set; }
    }

    public enum Situacao
    {
        Agendado = 1,
        Confirmado = 2,
        Concluido = 3,
        Cancelado = 4
    }
}
