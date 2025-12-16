namespace PhotoStudio.app.Models
{
    public class EnsaioModel
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required TipoEnsaio Tipo { get; set; }
        public required DateTime Data { get; set; }
        public required string Local { get; set; }
        public double? Valor { get; set; }
        public required int ClienteId { get; set; }
    }

    public enum TipoEnsaio
    {
        Casamento,
        Aniversario,
        Retrato,
        Corporativo,
        Outros
    }
}
