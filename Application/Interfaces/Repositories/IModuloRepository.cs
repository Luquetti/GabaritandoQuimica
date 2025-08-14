using Domain.Entities.Conteudo;

namespace Application.Interfaces.Repositories
{
    public interface IModuloRepository
    {
        Task<Modulo?> BuscarPorIdAsync(int id);
        Task<IEnumerable<Modulo>> BuscarTodosAsync();
        Task<IEnumerable<Modulo>> BuscarPorSerieAsync(int serieId);
        Task<Modulo?> BuscarPorNomeAsync(string nome);
        Task<bool> ExisteOrdemNaSerieAsync(int serieId, int ordem, int? idExcluir = null);
        Task<Modulo> CriarAsync(Modulo modulo);
        Task<Modulo> AtualizarAsync(Modulo modulo);
        Task<bool> DeletarAsync(int id);
        Task<int> ContarPorSerieAsync(int serieId);
        Task<int> ContarTotalAsync();
    }
}