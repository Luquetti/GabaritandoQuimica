using Application.DTO.Matricula;

namespace Application.Interfaces.Services
{
    public interface IMatriculaService
    {
        Task<MatriculaDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<MatriculaDto>> ListarPorAlunoAsync(int alunoId);
        Task<IEnumerable<MatriculaDto>> ListarExpirandoAsync(int diasAntecedencia);
        Task<bool> MatricularAlunoAsync(MatricularAlunoDto dto);
        Task<bool> RenovarMatriculaAsync(int matriculaId, int tipoPlano);
        Task<bool> CancelarMatriculaAsync(int matriculaId);
        Task<bool> AlunoTemAcessoAsync(int alunoId, int serieId);
        Task ProcessarMatriculasExpirandoAsync();
    }
}