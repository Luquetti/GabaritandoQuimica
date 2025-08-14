namespace Domain.Entities.Questoes
{
    public class RespostaQuestao
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int QuestaoId { get; set; }
        public string? RespostaEscolhida { get; set; }
        public bool? Correto { get; set; }
        public int PontosObtidos { get; set; }
        public DateTime DataResposta { get; set; }
        public int? TempoResposta { get; set; } 

        public Usuario Aluno { get; set; } 
        public BancoQuestao Questao { get; set; }
    }
}
