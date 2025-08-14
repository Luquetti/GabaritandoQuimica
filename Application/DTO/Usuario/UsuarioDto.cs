namespace Application.DTO.Usuario
{
    public  class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } 
        public string Email { get; set; } 
        public int TipoUsuario { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
