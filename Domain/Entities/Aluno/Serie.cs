using Domain.Entities.Conteudo;

namespace Domain.Entities.Aluno
{
    public class Serie
    {
        public int Id { get; set; }
        public string Nome { get; set; } 
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public List<Modulo> Modulos { get; set; }
        public List<Matricula> Matriculas { get; set; } 
    }
}
