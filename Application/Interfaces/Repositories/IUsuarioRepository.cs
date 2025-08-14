using Domain.Entities;
using Domain.Enum;

namespace Application.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> BuscarTodosAsync();
        Task<Usuario?> BuscarPorIdAsync(int id);
        Task<Usuario?> BuscarPorEmailAsync(string email);
        Task<Usuario?> BuscarPorEmailLoginAsync(string email);
        Task<bool> EmailExisteAsync(string email, int? idExcluir = null);
        Task<Usuario> CriarAsync(Usuario usuario);
        Task<Usuario> AtualizarAsync(Usuario usuario);
        Task<bool> DeletarAsync(int id);
        Task AtualizarUltimoLoginAsync(int id);
        Task<IEnumerable<Usuario>> BuscarPorTipoAsync(TipoUsuario tipo);
        Task<int> ContarTotalAsync();
        Task<int> ContarPorTipoAsync(TipoUsuario tipo);
    }
}