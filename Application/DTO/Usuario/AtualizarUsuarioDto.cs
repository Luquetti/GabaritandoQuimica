namespace Application.DTO.Usuario
{
    public class AtualizarUsuarioDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; } 
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool? Ativo { get; set; }
    }
}
