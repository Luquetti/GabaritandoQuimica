using Domain.Entities.Aluno;

namespace Domain.Entities.Conteudo
{
    public  class Aula
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public string Titulo { get; set; } 
        public string? Descricao { get; set; }
        public string? UrlVideo { get; set; }
        public int? DuracaoMinutos { get; set; }
        public int Ordem { get; set; }
        public DateTime DataCriacao { get; set; } 
        public bool Ativo { get; set; } 

        public Modulo Modulo { get; set; }
        public List<ProgressoAula> ProgressoAulas { get; set; }
        public List<Duvida> Duvidas { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
