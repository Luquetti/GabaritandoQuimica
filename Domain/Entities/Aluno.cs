namespace Domain.Entities
{
    public class Aluno
    {
        public int UsuarioId { get; set; }
        public int? AnoEscolar { get; set; }
        public string? PlanoAtivo { get; set; }
        public string StatusPagamento { get; set; } 
        public DateTime DataMatricula { get; set; }

        // Navegação
        public Usuario Usuario { get; set; } = null!;
    }
}
