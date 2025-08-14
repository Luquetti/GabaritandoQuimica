using Domain.Entities.Aluno;

namespace Application.Interfaces.Repositories
{
    public interface ISerieRepository
    {
        Task<IEnumerable<Serie>> BuscarTodasAsync();
        Task<IEnumerable<Serie>> BuscarAtivasAsync();
        Task<Serie?> BuscarPorIdAsync(int id);
        Task<Serie?> BuscarPorNomeAsync(string nome);
        Task<bool> ExisteOrdemAsync(int ordem, int? idExcluir = null);
        Task<Serie> CriarAsync(Serie serie);
        Task<Serie> AtualizarAsync(Serie serie);
        Task<bool> DeletarAsync(int id);
        Task<int> ContarTotalAsync();
        Task<int> ContarAtivasAsync();
        Task<decimal> ObterPrecoMedioAsync();
    }
}