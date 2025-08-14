namespace Application.DTO.Modulo
{
    public  class CriarModuloDto
    {
        public int SerieId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
    }
}
