using Application.Interfaces.Repositories;
using Domain.Entities.Aluno;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class SerieRepository : ISerieRepository
    {
        private readonly PlataformaDbContext _context;

        public SerieRepository(PlataformaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Serie>> BuscarTodasAsync()
        {
            return await _context.Series
                .AsNoTracking()
                .OrderBy(s => s.Ordem)
                .ToListAsync();
        }

        public async Task<IEnumerable<Serie>> BuscarAtivasAsync()
        {
            return await _context.Series
                .AsNoTracking()
                .Where(s => s.Ativo)
                .OrderBy(s => s.Ordem)
                .ToListAsync();
        }

        public async Task<Serie?> BuscarPorIdAsync(int id)
        {
            return await _context.Series
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Serie?> BuscarPorNomeAsync(string nome)
        {
            return await _context.Series
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Nome.ToLower() == nome.ToLower());
        }

        public async Task<bool> ExisteOrdemAsync(int ordem, int? idExcluir = null)
        {
            var query = _context.Series.Where(s => s.Ordem == ordem);

            if (idExcluir.HasValue)
            {
                query = query.Where(s => s.Id != idExcluir.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Serie> CriarAsync(Serie serie)
        {
            _context.Series.Add(serie);
            await _context.SaveChangesAsync();
            return serie;
        }

        public async Task<Serie> AtualizarAsync(Serie serie)
        {
            _context.Entry(serie).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return serie;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var serie = await _context.Series.FindAsync(id);
            if (serie == null) return false;

            // Soft delete - apenas desativa
            serie.Ativo = false;
            serie.DataCriacao = DateTime.UtcNow; // No banco não tem DataAlteracao para Serie

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ContarTotalAsync()
        {
            return await _context.Series.CountAsync();
        }

        public async Task<int> ContarAtivasAsync()
        {
            return await _context.Series.CountAsync(s => s.Ativo);
        }

        public async Task<decimal> ObterPrecoMedioAsync()
        {
            var series = await _context.Series
                .Where(s => s.Ativo)
                .AsNoTracking()
                .ToListAsync();

            return series.Any() ? series.Average(s => s.Preco) : 0;
        }
    }
}