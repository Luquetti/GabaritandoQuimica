namespace Domain.Entities
{
    public class Professor
    {
        public int UsuarioId { get; set; }
        public string Materia { get; set; } = string.Empty;
        public string? Biografia { get; set; }
        public DateTime DataContratacao { get; set; }

        // Navegação
        public Usuario Usuario { get; set; } = null!;
    }
}
