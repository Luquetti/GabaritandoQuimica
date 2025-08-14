using Application.DTO.Serie;

namespace Application.Interfaces.Services
{
    public interface ISerieService
    {
        Task<SerieDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<SerieDto>> ListarAtivasAsync();
        Task<IEnumerable<SerieDto>> ListarTodasAsync();
        Task<int> CriarAsync(CriarSerieDto dto);
        Task<bool> AtualizarAsync(AtualizarSerieDto dto);
        Task<bool> DesativarAsync(int id);
    }
}