using Domain.DTOs;
using Domain.Interfaces.Services;
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarAluno([FromBody] CadastrarAlunoDto dto)
        {
            var usuarioId = await _usuarioService.CadastrarAlunoAsync(dto);
            return Ok(new
            {
                id = usuarioId,
                message = "Aluno cadastrado com sucesso! Faça login para continuar.",
                email = dto.Email.ToLower()
            });
        }
    }
}