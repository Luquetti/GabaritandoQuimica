using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> BuscarPorEmailAsync(string email);
        Task<Usuario?> BuscarPorIdAsync(int id);
        Task<bool> EmailExisteAsync(string email);

        Task<Usuario> CriarUsuarioAsync(Usuario usuario);
        Task<Usuario> AtualizarUsuarioAsync(Usuario usuario);
        Task<Aluno> CriarAlunoAsync(Aluno aluno);
        Task<Aluno> AtualizarAlunoAsync(Aluno aluno);
    }
}