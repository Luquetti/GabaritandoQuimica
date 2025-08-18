using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class AlunoController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;
        private readonly IValidator<AlterarSenhaDto> _senhaValidator;

        public AlunoController(IUsuarioRepository repository, IValidator<AlterarSenhaDto> senhaValidator)
        {
            _repository = repository;
            _senhaValidator = senhaValidator;
        }

        /// <summary>
        /// Buscar meu perfil de aluno
        /// </summary>
        [HttpGet("perfil")]
        public async Task<IActionResult> MeuPerfil()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst("usuario_id")!.Value);

                var usuario = await _repository.BuscarPorIdAsync(usuarioId);
                if (usuario == null)
                    return NotFound(new { erro = "Usuário não encontrado" });

                return Ok(new
                {
                    id = usuario.Id,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    telefone = usuario.Telefone,
                    dataNascimento = usuario.DataNascimento,
                    cidade = usuario.Cidade,
                    ativo = usuario.Ativo,
                    ultimoLogin = usuario.UltimoLogin,
                    dataCriacao = usuario.DataCriacao,
                    aluno = usuario.Aluno != null ? new
                    {
                        anoEscolar = usuario.Aluno.AnoEscolar,
                        planoAtivo = usuario.Aluno.PlanoAtivo,
                        statusPagamento = usuario.Aluno.StatusPagamento,
                        dataMatricula = usuario.Aluno.DataMatricula
                    } : null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno do servidor" });
            }
        }


        [HttpPut("perfil")]
        public async Task<IActionResult> AtualizarPerfil([FromBody] Usuario dadosAtualizacao)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst("usuario_id")!.Value);

                var usuario = await _repository.BuscarPorIdAsync(usuarioId);
                if (usuario == null)
                    return NotFound(new { erro = "Usuário não encontrado" });

                if (!string.IsNullOrWhiteSpace(dadosAtualizacao.Nome))
                    usuario.Nome = dadosAtualizacao.Nome.Trim();

                if (!string.IsNullOrWhiteSpace(dadosAtualizacao.Telefone))
                    usuario.Telefone = dadosAtualizacao.Telefone.Trim();

                if (!string.IsNullOrWhiteSpace(dadosAtualizacao.Cidade))
                    usuario.Cidade = dadosAtualizacao.Cidade.Trim();

                if (dadosAtualizacao.DataNascimento.HasValue)
                    usuario.DataNascimento = dadosAtualizacao.DataNascimento;

                await _repository.AtualizarUsuarioAsync(usuario);

                return Ok(new { message = "Perfil atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno do servidor" });
            }
        }

        [HttpPut("senha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDto dto)
        {
            try
            {
                await ValidationHelper.ValidateAsync(_senhaValidator, dto, "AlterarSenha");

                var usuarioId = int.Parse(User.FindFirst("usuario_id")!.Value);

                var usuario = await _repository.BuscarPorIdAsync(usuarioId);
                if (usuario == null)
                    return NotFound(new { erro = "Usuário não encontrado" });

                if (!BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, usuario.SenhaHash))
                    return BadRequest(new { erro = "Senha atual incorreta" });

                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
                await _repository.AtualizarUsuarioAsync(usuario);

                return Ok(new { message = "Senha alterada com sucesso!" });
            }
            catch (Domain.Validators.ValidationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno do servidor" });
            }
        }

        [HttpDelete("conta")]
        public async Task<IActionResult> DeletarConta()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst("usuario_id")!.Value);

                var usuario = await _repository.BuscarPorIdAsync(usuarioId);
                if (usuario == null)
                    return NotFound(new { erro = "Usuário não encontrado" });

                // SOFT DELETE - só marca como inativo
                usuario.Ativo = false;
                await _repository.AtualizarUsuarioAsync(usuario);

                return Ok(new { message = "Conta desativada com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno do servidor" });
            }
        }
    }
}