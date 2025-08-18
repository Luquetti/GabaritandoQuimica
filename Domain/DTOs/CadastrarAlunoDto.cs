namespace Domain.DTOs
{
    public class CadastrarAlunoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Cidade { get; set; }
        public int? AnoEscolar { get; set; }
    }
}
