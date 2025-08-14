
using Application.DTO.Usuario;
using Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace PlataformaQuimica.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET api/usuario
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var usuarios = await _usuarioService.ListarTodosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        // GET api/usuario/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObterPorIdAsync(id);

                if (usuario == null)
                    return NotFound(new { message = "Usuário não encontrado" });

                return Ok(usuario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        // POST api/usuario/cadastrar-aluno
        [HttpPost("cadastrar-aluno")]
        public async Task<IActionResult> CadastrarAluno([FromBody] CriarUsuarioDto dto)
        {
            try
            {
                var id = await _usuarioService.CriarAlunoAsync(dto);

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new { id },
                    new { id, message = "Aluno cadastrado com sucesso" }
                );
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new { message = "Dados inválidos", errors });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        // POST api/usuario/cadastrar-professor
        [HttpPost("cadastrar-professor")]
        public async Task<IActionResult> CadastrarProfessor([FromBody] CriarUsuarioDto dto)
        {
            try
            {
                var id = await _usuarioService.CriarProfessorAsync(dto);

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new { id },
                    new { id, message = "Professor cadastrado com sucesso" }
                );
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new { message = "Dados inválidos", errors });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        // POST api/usuario/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var usuario = await _usuarioService.AutenticarAsync(dto);

                if (usuario == null)
                    return Unauthorized(new { message = "Email ou senha incorretos" });

                // Aqui você adicionaria a geração do JWT token
                return Ok(new
                {
                    message = "Login realizado com sucesso",
                    usuario = new
                    {
                        usuario.Id,
                        usuario.Nome,
                        usuario.Email,
                        usuario.TipoUsuario
                    }
                    // token = "jwt_token_aqui" // Implementar depois
                });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new { message = "Dados inválidos", errors });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        // PUT api/usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarUsuarioDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(new { message = "ID da URL não confere com o ID do objeto" });

                var sucesso = await _usuarioService.AtualizarAsync(dto);

                if (!sucesso)
                    return NotFound(new { message = "Usuário não encontrado" });

                return Ok(new { message = "Usuário atualizado com sucesso" });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desativar(int id)
        {
            try
            {
                var sucesso = await _usuarioService.DesativarAsync(id);

                if (!sucesso)
                    return NotFound(new { message = "Usuário não encontrado" });

                return Ok(new { message = "Usuário desativado com sucesso" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        [HttpGet("verificar-email/{email}")]
        public async Task<IActionResult> VerificarEmail(string email)
        {
            try
            {
                var existe = await _usuarioService.EmailExisteAsync(email);

                return Ok(new { existe, message = existe ? "Email já cadastrado" : "Email disponível" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        [HttpPost("{id}/alterar-senha")]
        public async Task<IActionResult> AlterarSenha(int id, [FromBody] object request)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(request.ToString()!);

                if (!json.ContainsKey("senhaAtual") || !json.ContainsKey("novaSenha"))
                    return BadRequest(new { message = "Campos 'senhaAtual' e 'novaSenha' são obrigatórios" });

                var sucesso = await _usuarioService.AlterarSenhaAsync(id, json["senhaAtual"], json["novaSenha"]);

                if (!sucesso)
                    return BadRequest(new { message = "Senha atual incorreta ou usuário não encontrado" });

                return Ok(new { message = "Senha alterada com sucesso" });
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "Funcionalidade ainda não implementada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }
    }
}