using Domain.Entities.Aluno;

namespace Domain.Entities.Questoes
{
    public class RespostaDuvida
    {
        public int Id { get; set; }
        public int DuvidaId { get; set; }
        public int ProfessorId { get; set; }
        public string Resposta { get; set; } 
        public DateTime DataResposta { get; set; }
        public bool Editada { get; set; } = false;
        public DateTime? DataEdicao { get; set; }

        // Navegação
        public Duvida Duvida { get; set; } 
        public Usuario Professor { get; set; }
    }
}
