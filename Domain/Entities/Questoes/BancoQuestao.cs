using Domain.Entities.Conteudo;
using Domain.Enum;

namespace Domain.Entities.Questoes
{
    public  class BancoQuestao
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public string? QuestaoExternaId { get; set; }
        public string? FonteOrigem { get; set; }
        public int? AnoVestibular { get; set; }
        public string Enunciado { get; set; } 
        public TipoQuestao TipoQuestao { get; set; }
        public DificuldadeQuestao Dificuldade { get; set; }
        public int PontosValor { get; set; }
        public string? Explicacao { get; set; }
        public string? TagsConteudo { get; set; }
        public DateTime DataImportacao { get; set; }
        public bool Ativo { get; set; }
        public Modulo Modulo { get; set; } 
        public List<AlternativaQuestao> Alternativas { get; set; } 
        public List<RespostaQuestao> Respostas { get; set; } 
    }
}
