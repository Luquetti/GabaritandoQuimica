using Application.DTO.Usuario;

namespace Application.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> ObterPorIdAsync(int id);
        Task<UsuarioDto?> AutenticarAsync(LoginDto dto);
        Task<IEnumerable<UsuarioDto>> ListarTodosAsync();
        Task<IEnumerable<UsuarioDto>> ListarProfessoresAsync();
        Task<IEnumerable<UsuarioDto>> ListarAlunosAsync();
        Task<int> CriarAlunoAsync(CriarUsuarioDto dto);
        Task<int> CriarProfessorAsync(CriarUsuarioDto dto);
        Task<bool> AtualizarAsync(AtualizarUsuarioDto dto);
        Task<bool> AlterarSenhaAsync(int id, string senhaAtual, string novaSenha);
        Task<bool> DesativarAsync(int id);
        Task<bool> EmailExisteAsync(string email);
    }
}