namespace Application.DTO.Serie
{
    public  class AtualizarSerieDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } 
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
    }
}
