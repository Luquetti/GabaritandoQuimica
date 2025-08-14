using Application.DTO.Usuario;
using BCrypt.Net;
using Domain.Entities;
using Domain.Enum;

namespace Infra.Extensions
{
    public static class UsuarioExtensions
    {
        // Converter CriarUsuarioDto para Entity
        public static Usuario CriarEntity(this CriarUsuarioDto dto)
        {
            return new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                TipoUsuario = (TipoUsuario)dto.TipoUsuario,
                Telefone = dto.Telefone,
                DataNascimento = dto.DataNascimento,
                DataCriacao = DateTime.UtcNow,
                Ativo = true
            };
        }

        // Converter Entity para DTO (Response)
        public static UsuarioDto ToDto(this Usuario entity)
        {
            return new UsuarioDto
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Email = entity.Email,
                TipoUsuario = (int)entity.TipoUsuario,
                Telefone = entity.Telefone,
                DataNascimento = entity.DataNascimento,
                DataCriacao = entity.DataCriacao,
                UltimoLogin = entity.UltimoLogin,
                Ativo = entity.Ativo
            };
        }

        // Aplicar alterações na entidade existente
        public static void AplicarAlteracoes(this Usuario entity, AtualizarUsuarioDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Email = dto.Email;
            entity.Telefone = dto.Telefone;
            entity.DataNascimento = dto.DataNascimento;
            entity.Ativo = dto.Ativo;
            // DataCriacao, Id, SenhaHash não mudam
        }

        // Verificar senha
        public static bool VerificarSenha(this Usuario entity, string senha)
        {
            return BCrypt.Net.BCrypt.Verify(senha, entity.SenhaHash);
        }

        // Alterar senha
        public static void AlterarSenha(this Usuario entity, string novaSenha)
        {
            entity.SenhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        }
    }
}