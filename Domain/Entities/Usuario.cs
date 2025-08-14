using Domain.Entities.Aluno;
using Domain.Enum;

namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } 
        public string Email { get; set; } 
        public string SenhaHash { get; set; } 
        public TipoUsuario TipoUsuario { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public DateTime DataCriacao { get; set; } 
        public DateTime? UltimoLogin { get; set; }
        public bool Ativo { get; set; }
        public List<Matricula> Matriculas { get; set; } 
        public List<ProgressoAula> ProgressoAulas { get; set; }
        public List<Duvida> Duvidas { get; set; } 
    }
}
