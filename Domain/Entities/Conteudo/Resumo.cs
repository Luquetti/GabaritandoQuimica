namespace Domain.Entities.Conteudo
{
    public  class Resumo
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public string Titulo { get; set; } 
        public string ConteudoResumo { get; set; } 
        public string? UrlArquivoPdf { get; set; }
        public DateTime DataCriacao { get; set; } 
        public Modulo Modulo { get; set; } 
        public bool Ativo { get; set; } 
        public DateTime? DataAlteracao { get; set; }
    }
}
