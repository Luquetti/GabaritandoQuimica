using Domain.Enum.EnumUsuario;

namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Cidade { get; set; }
        public EnumTipoUsuario TipoUsuario { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public Administrador? Administrador { get; set; }
        public Professor? Professor { get; set; }
        public Aluno? Aluno { get; set; }
    }
}
