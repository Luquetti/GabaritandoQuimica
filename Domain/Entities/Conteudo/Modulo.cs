using Domain.Entities.Aluno;
using Domain.Entities.Questoes;

namespace Domain.Entities.Conteudo
{
    public class Modulo
    {
        public int Id { get; set; }
        public int SerieId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
        public DateTime? DataCriacao { get; set; }
        public Serie Serie { get; set; }
        public List<Aula> Aulas { get; set; } 
        public List<BancoQuestao> Questoes { get; set; }
        public List<Resumo> Resumos { get; set; }
    }
}
