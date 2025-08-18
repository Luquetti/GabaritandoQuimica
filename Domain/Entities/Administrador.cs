namespace Domain.Entities
{
    public class Administrador
    {
        public int UsuarioId { get; set; }
        public int NivelAcesso { get; set; } = 1;
        public DateTime DataPromocao { get; set; }

        // Navegação
        public Usuario Usuario { get; set; } = null!;
    }
}
