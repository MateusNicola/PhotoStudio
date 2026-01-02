using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoStudio.app.Models
{
    [Table("categorias")]
    public class CategoriaModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O código é obrigatório")]
        [StringLength(5, ErrorMessage = "O código deve ter no máximo 5 caracteres")]
        [Column("codigo")]
        public required string Codigo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(250, ErrorMessage = "A descrição deve ter no máximo 250 caracteres")]
        [Column("descricao")]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "A cor é obrigatória")]
        [StringLength(7, ErrorMessage = "A cor deve ter no máximo 7 caracteres")]
        [Column("cor")]
        public required string Cor { get; set; }

        public int UsuarioId { get; set; }

        [Column("criado_em")]
        public DateTime CriadoEm { get; set; } = DateTime.Now;
    }
}
