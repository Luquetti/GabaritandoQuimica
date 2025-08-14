using Domain.Entities.Aluno;
using Domain.Enum;

namespace Application.Interfaces.Repositories
{
    public interface IMatriculaRepository
    {
        Task<Matricula?> BuscarPorIdAsync(int id);
        Task<Matricula?> BuscarPorAlunoESerieAsync(int alunoId, int serieId);
        Task<IEnumerable<Matricula>> BuscarPorAlunoAsync(int alunoId);
        Task<IEnumerable<Matricula>> BuscarPorSerieAsync(int serieId);
        Task<IEnumerable<Matricula>> BuscarExpirandoAsync(int diasAntecedencia);
        Task<IEnumerable<Matricula>> BuscarPorStatusAsync(StatusMatricula status);
        Task<bool> AlunoTemAcessoSerieAsync(int alunoId, int serieId);
        Task<Matricula> CriarAsync(Matricula matricula);
        Task<Matricula> AtualizarAsync(Matricula matricula);
        Task<bool> RenovarMatriculaAsync(int id, DateTime novaDataExpiracao);
        Task<bool> DeletarAsync(int id);
        Task<int> ContarAtivasAsync();
        Task<int> ContarPorStatusAsync(StatusMatricula status);
    }
}