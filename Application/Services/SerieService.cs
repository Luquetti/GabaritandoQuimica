using Application.DTO.Serie;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using FluentValidation;

namespace Application.Services
{
    public class SerieService : ISerieService
    {
        private readonly ISerieRepository _repository;
        private readonly IValidator<CriarSerieDto> _criarValidator;

        public SerieService(
            ISerieRepository repository,
            IValidator<CriarSerieDto> criarValidator)
        {
            _repository = repository;
            _criarValidator = criarValidator;
        }

        public async Task<SerieDto?> ObterPorIdAsync(int id)
        {
            // TODO: Implementar depois - por enquanto só para compilar
            throw new NotImplementedException("Será implementado quando testarmos usuários");
        }

        public async Task<IEnumerable<SerieDto>> ListarAtivasAsync()
        {
            // TODO: Implementar depois
            throw new NotImplementedException("Será implementado quando testarmos usuários");
        }

        public async Task<IEnumerable<SerieDto>> ListarTodasAsync()
        {
            // TODO: Implementar depois
            throw new NotImplementedException("Será implementado quando testarmos usuários");
        }

        public async Task<int> CriarAsync(CriarSerieDto dto)
        {
            // TODO: Implementar depois
            throw new NotImplementedException("Será implementado quando testarmos usuários");
        }

        public async Task<bool> AtualizarAsync(AtualizarSerieDto dto)
        {
            // TODO: Implementar depois
            throw new NotImplementedException("Será implementado quando testarmos usuários");
        }

        public async Task<bool> DesativarAsync(int id)
        {
            // TODO: Implementar depois
            throw new NotImplementedException("Será implementado quando testarmos usuários");
        }
    }
}