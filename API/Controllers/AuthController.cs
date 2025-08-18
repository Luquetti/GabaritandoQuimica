using Domain.DTOs;
using Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioService _usuarioService;

        public AuthController(IAuthService authService, IUsuarioService usuarioService)
        {
            _authService = authService;
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Fazer login na plataforma
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _authService.LoginAsync(dto);
                return Ok(new
                {
                    token,
                    expiresIn = 24 * 60 * 60, 
                    message = "Login realizado com sucesso!",
                    type = "Bearer"
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Cadastrar novo aluno na plataforma
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarAluno([FromBody] CadastrarAlunoDto dto)
        {
            try
            {
                var usuarioId = await _usuarioService.CadastrarAlunoAsync(dto);
                return Ok(new
                {
                    id = usuarioId,
                    message = "Aluno cadastrado com sucesso! Faça login para continuar.",
                    email = dto.Email.ToLower()
                });
            }
            catch (Domain.Validators.ValidationException ex)
            {
                return BadRequest(new { erro = ex.Message, tipo = "Validação" });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { erro = ex.Message, tipo = "Conflito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno do servidor" });
            }
        }
    }
}