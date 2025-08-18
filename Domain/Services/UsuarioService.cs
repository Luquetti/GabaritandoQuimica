
using Domain.DTOs;
using Domain.Entities;
using Domain.Enum.EnumUsuario;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Validators;
using FluentValidation;

namespace Domain.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IValidator<CadastrarAlunoDto> _cadastroValidator;

        public UsuarioService(
            IUsuarioRepository repository,
            IValidator<CadastrarAlunoDto> cadastroValidator)
        {
            _repository = repository;
            _cadastroValidator = cadastroValidator;
        }

        public async Task<int> CadastrarAlunoAsync(CadastrarAlunoDto dto)
        {
            try
            {
                await ValidationHelper.ValidateAsync(_cadastroValidator, dto, "Cadastro");

                if (await _repository.EmailExisteAsync(dto.Email))
                    throw new InvalidOperationException("Email já está cadastrado");

                var usuario = new Usuario
                {
                    Nome = dto.Nome.Trim(),
                    Email = dto.Email.Trim().ToLower(),
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                    Telefone = !string.IsNullOrEmpty(dto.Telefone) ? dto.Telefone.Trim() : null,
                    DataNascimento = dto.DataNascimento?.ToUniversalTime(), 
                    Cidade = !string.IsNullOrEmpty(dto.Cidade) ? dto.Cidade.Trim() : null,
                    TipoUsuario = EnumTipoUsuario.Aluno,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow, 
                    UltimoLogin = null
                };

                var usuarioCriado = await _repository.CriarUsuarioAsync(usuario);

                var aluno = new Aluno
                {
                    UsuarioId = usuarioCriado.Id,
                    AnoEscolar = dto.AnoEscolar,
                    PlanoAtivo = null, 
                    StatusPagamento = "pendente",
                    DataMatricula = DateTime.UtcNow 
                };

                await _repository.CriarAlunoAsync(aluno);

                return usuarioCriado.Id;
            }
            catch (Domain.Validators.ValidationException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado no cadastro: {ex.Message}");
                throw new Exception("Erro interno ao cadastrar usuário");
            }
        }
    }
}