using Domain.DTOs;

namespace Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<int> CadastrarAlunoAsync(CadastrarAlunoDto dto);
    }
}