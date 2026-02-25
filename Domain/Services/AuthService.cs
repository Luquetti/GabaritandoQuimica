using Domain.DTOs;
using Domain.Entities;
using Domain.Enum.EnumUsuario;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IValidator<LoginDto> _loginValidator;

        public AuthService(
            IUsuarioRepository repository,
            IConfiguration configuration,
            IValidator<LoginDto> loginValidator)
        {
            _repository = repository;
            _configuration = configuration;
            _loginValidator = loginValidator;
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            await ValidationHelper.ValidateAsync(_loginValidator, dto, "Login");

            var usuario = await _repository.BuscarPorEmailAsync(dto.Email);

            if (usuario == null || !usuario.Ativo)
                throw new UnauthorizedAccessException("Email ou senha incorretos");

            if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                throw new UnauthorizedAccessException("Email ou senha incorretos");

            usuario.UltimoLogin = DateTime.UtcNow;
            await _repository.AtualizarUsuarioAsync(usuario);

            return GerarToken(usuario);
        }

        private string GerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new("usuario_id", usuario.Id.ToString()),
                new("email", usuario.Email),
                new("nome", usuario.Nome),
                new("tipo_usuario", usuario.TipoUsuario.ToString())
            };

            switch (usuario.TipoUsuario)
            {
                case EnumTipoUsuario.Aluno:
                    if (usuario.Aluno != null)
                    {
                        claims.Add(new("ano_escolar", usuario.Aluno.AnoEscolar?.ToString() ?? ""));
                        claims.Add(new("plano_ativo", usuario.Aluno.PlanoAtivo ?? ""));
                        claims.Add(new("status_pagamento", usuario.Aluno.StatusPagamento));
                        claims.Add(new("permission", "course.view"));
                        claims.Add(new("permission", "profile.own"));
                    }
                    break;

                case EnumTipoUsuario.Professor:
                    if (usuario.Professor != null)
                    {
                        claims.Add(new("materia", usuario.Professor.Materia));
                        claims.Add(new("data_contratacao", usuario.Professor.DataContratacao.ToString("yyyy-MM-dd")));
                        claims.Add(new("permission", "course.manage"));
                        claims.Add(new("permission", "students.view"));
                        claims.Add(new("permission", "profile.own"));
                    }
                    break;

                case EnumTipoUsuario.Admin:
                    if (usuario.Administrador != null)
                    {
                        claims.Add(new("nivel_acesso", usuario.Administrador.NivelAcesso.ToString()));
                        claims.Add(new("permission", "users.manage"));
                        claims.Add(new("permission", "system.admin"));
                        claims.Add(new("permission", "profile.own"));
                    }
                    break;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    
    }
}
