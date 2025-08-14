using Domain.Entities.Questoes;
using Domain.Enum;

namespace Application.Interfaces.Repositories
{
    public interface IBancoQuestaoRepository
    {
        Task<BancoQuestao?> BuscarPorIdAsync(int id);
        Task<IEnumerable<BancoQuestao>> BuscarPorModuloAsync(int moduloId);
        Task<IEnumerable<BancoQuestao>> BuscarPorDificuldadeAsync(DificuldadeQuestao dificuldade);
        Task<IEnumerable<BancoQuestao>> BuscarPorFonteAsync(string fonte);
        Task<IEnumerable<BancoQuestao>> BuscarAtivasAsync();
        Task<bool> ExisteQuestaoExternaAsync(string questaoExternaId);
        Task<BancoQuestao> CriarAsync(BancoQuestao questao);
        Task<BancoQuestao> AtualizarAsync(BancoQuestao questao);
        Task<bool> DeletarAsync(int id);
        Task<int> ContarPorModuloAsync(int moduloId);
        Task<int> ContarPorDificuldadeAsync(DificuldadeQuestao dificuldade);
    }
}