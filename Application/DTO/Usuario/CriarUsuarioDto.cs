namespace Application.DTO.Usuario
{
    public  class CriarUsuarioDto
    {
        public string Nome { get; set; } 
        public string Email { get; set; }
        public string Senha { get; set; }
        public int TipoUsuario { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
    }
}
