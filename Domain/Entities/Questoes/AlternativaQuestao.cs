namespace Domain.Entities.Questoes
{
    public class AlternativaQuestao
    {
        public int Id { get; set; }
        public int QuestaoId { get; set; }
        public char Letra { get; set; }
        public string Texto { get; set; }
        public bool Correta { get; set; }

        public BancoQuestao Questao { get; set; } 
    }
}
