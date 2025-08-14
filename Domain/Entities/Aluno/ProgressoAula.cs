using Domain.Entities.Conteudo;

namespace Domain.Entities.Aluno
{
    public class ProgressoAula
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int AulaId { get; set; }
        public decimal PercentualAssistido { get; set; }
        public bool Concluida { get; set; } = false;
        public int TempoAssistido { get; set; } 
        public DateTime UltimoAcesso { get; set; } 

        public Usuario Aluno { get; set; } 
        public Aula Aula { get; set; } 
    }
}
