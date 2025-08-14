using Domain.Entities.Conteudo;
using Domain.Entities.Questoes;
using Domain.Enum;

namespace Domain.Entities.Aluno
{
    public class Duvida
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int? AulaId { get; set; }
        public int? ModuloId { get; set; }
        public int? QuestaoId { get; set; }
        public string Titulo { get; set; }
        public string Pergunta { get; set; }
        public StatusDuvida Status { get; set; } 
        public PrioridadeDuvida Prioridade { get; set; } 
        public DateTime DataCriacao { get; set; } 
        public DateTime? DataResposta { get; set; }

        public Usuario Aluno { get; set; } = null!;
        public Aula? Aula { get; set; }
        public Modulo? Modulo { get; set; }
        public BancoQuestao? Questao { get; set; }
        public List<RespostaDuvida> Respostas { get; set; } 
    }
}
