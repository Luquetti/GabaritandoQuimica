using Domain.Entities.Conteudo;

namespace Application.Interfaces.Repositories
{
    public interface IAulaRepository
    {
        Task<Aula?> BuscarPorIdAsync(int id);
        Task<IEnumerable<Aula>> BuscarPorModuloAsync(int moduloId);
        Task<IEnumerable<Aula>> BuscarPorSerieAsync(int serieId);
        Task<IEnumerable<Aula>> BuscarAtivasAsync();
        Task<bool> ExisteOrdemNoModuloAsync(int moduloId, int ordem, int? idExcluir = null);
        Task<Aula> CriarAsync(Aula aula);
        Task<Aula> AtualizarAsync(Aula aula);
        Task<bool> DeletarAsync(int id);
        Task<int> ContarPorModuloAsync(int moduloId);
        Task<int> ContarAtivasAsync();
    }
}