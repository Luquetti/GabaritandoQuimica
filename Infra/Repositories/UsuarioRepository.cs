using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly PlataformaDbContext _context;

        public UsuarioRepository(PlataformaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> BuscarTodosAsync()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.Ativo)
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<Usuario?> BuscarPorIdAsync(int id)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
        }

        public async Task<Usuario?> BuscarPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.Ativo);
        }

        public async Task<Usuario?> BuscarPorEmailLoginAsync(string email)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.Ativo);
        }

        public async Task<bool> EmailExisteAsync(string email, int? idExcluir = null)
        {
            var query = _context.Usuarios.Where(u => u.Email.ToLower() == email.ToLower() && u.Ativo);

            if (idExcluir.HasValue)
            {
                query = query.Where(u => u.Id != idExcluir.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Usuario> CriarAsync(Usuario usuario)
        {
            usuario.DataCriacao = DateTime.UtcNow;
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> AtualizarAsync(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            usuario.Ativo = false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AtualizarUltimoLoginAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.UltimoLogin = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Usuario>> BuscarPorTipoAsync(Domain.Enum.TipoUsuario tipo)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.TipoUsuario == tipo && u.Ativo)
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<int> ContarTotalAsync()
        {
            return await _context.Usuarios.CountAsync(u => u.Ativo);
        }

        public async Task<int> ContarPorTipoAsync(Domain.Enum.TipoUsuario tipo)
        {
            return await _context.Usuarios.CountAsync(u => u.TipoUsuario == tipo && u.Ativo);
        }
    }
}