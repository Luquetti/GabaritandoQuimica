using Application.DTO.Usuario;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Enum;
using FluentValidation;
using Infra.Extensions;
namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IValidator<CriarUsuarioDto> _criarValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public UsuarioService(
            IUsuarioRepository repository,
            IValidator<CriarUsuarioDto> criarValidator,
            IValidator<LoginDto> loginValidator)
        {
            _repository = repository;
            _criarValidator = criarValidator;
            _loginValidator = loginValidator;
        }

        public async Task<UsuarioDto?> ObterPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID deve ser maior que zero", nameof(id));

            var usuario = await _repository.BuscarPorIdAsync(id);
            return usuario?.ToDto();
        }

        public async Task<UsuarioDto?> AutenticarAsync(LoginDto dto)
        {
            // Validar entrada
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Buscar usuário por email
            var usuario = await _repository.BuscarPorEmailLoginAsync(dto.Email);
            if (usuario == null)
                return null; // Email não encontrado

            // Verificar se usuário está ativo
            if (!usuario.Ativo)
                throw new InvalidOperationException("Usuário desativado");

            // Verificar senha usando extension
            if (!usuario.VerificarSenha(dto.Senha))
                return null; // Senha incorreta

            // Atualizar último login
            await _repository.AtualizarUltimoLoginAsync(usuario.Id);

            return usuario.ToDto();
        }

        public async Task<IEnumerable<UsuarioDto>> ListarTodosAsync()
        {
            var usuarios = await _repository.BuscarTodosAsync();
            return usuarios.Select(u => u.ToDto());
        }

        public async Task<IEnumerable<UsuarioDto>> ListarProfessoresAsync()
        {
            var professores = await _repository.BuscarPorTipoAsync(TipoUsuario.Professor);
            return professores.Select(p => p.ToDto());
        }

        public async Task<IEnumerable<UsuarioDto>> ListarAlunosAsync()
        {
            var alunos = await _repository.BuscarPorTipoAsync(TipoUsuario.Aluno);
            return alunos.Select(a => a.ToDto());
        }

        public async Task<int> CriarAlunoAsync(CriarUsuarioDto dto)
        {
            // Validar entrada
            var validationResult = await _criarValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Verificar se email já existe
            if (await _repository.EmailExisteAsync(dto.Email))
                throw new InvalidOperationException("Email já está cadastrado no sistema");

            // Definir tipo como aluno
            dto.TipoUsuario = (int)TipoUsuario.Aluno;

            // Validar senha (regras de negócio)
            ValidarSenha(dto.Senha);

            // Converter para entidade usando extension renomeada
            var usuario = dto.CriarEntity();
            usuario.TipoUsuario = TipoUsuario.Aluno;

            var usuarioCriado = await _repository.CriarAsync(usuario);
            return usuarioCriado.Id;
        }

        public async Task<int> CriarProfessorAsync(CriarUsuarioDto dto)
        {
            // Validar entrada
            var validationResult = await _criarValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Verificar se email já existe
            if (await _repository.EmailExisteAsync(dto.Email))
                throw new InvalidOperationException("Email já está cadastrado no sistema");

            // Definir tipo como professor
            dto.TipoUsuario = (int)TipoUsuario.Professor;

            // Validar senha (regras de negócio)
            ValidarSenha(dto.Senha);

            // Converter para entidade usando extension renomeada
            var usuario = dto.CriarEntity();
            usuario.TipoUsuario = TipoUsuario.Professor;

            var usuarioCriado = await _repository.CriarAsync(usuario);
            return usuarioCriado.Id;
        }

        public async Task<bool> AtualizarAsync(AtualizarUsuarioDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID deve ser maior que zero", nameof(dto.Id));

            // Verificar se usuário existe
            var usuarioExistente = await _repository.BuscarPorIdAsync(dto.Id);
            if (usuarioExistente == null)
                throw new InvalidOperationException("Usuário não encontrado");

            // Verificar se email já está em uso por outro usuário
            if (await _repository.EmailExisteAsync(dto.Email, dto.Id))
                throw new InvalidOperationException("Email já está em uso por outro usuário");

            // Validar dados
            ValidarDadosAtualizacao(dto);

            // Aplicar alterações usando extension renomeada
            usuarioExistente.AplicarAlteracoes(dto);
            await _repository.AtualizarAsync(usuarioExistente);

            return true;
        }

        public async Task<bool> AlterarSenhaAsync(int id, string senhaAtual, string novaSenha)
        {
            if (id <= 0)
                throw new ArgumentException("ID deve ser maior que zero", nameof(id));

            if (string.IsNullOrWhiteSpace(senhaAtual))
                throw new ArgumentException("Senha atual é obrigatória", nameof(senhaAtual));

            if (string.IsNullOrWhiteSpace(novaSenha))
                throw new ArgumentException("Nova senha é obrigatória", nameof(novaSenha));

            // Verificar se usuário existe
            var usuario = await _repository.BuscarPorIdAsync(id);
            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado");

            // Verificar senha atual usando extension
            if (!usuario.VerificarSenha(senhaAtual))
                throw new InvalidOperationException("Senha atual incorreta");

            // Validar nova senha
            ValidarSenha(novaSenha);

            // Alterar senha usando extension
            usuario.AlterarSenha(novaSenha);
            await _repository.AtualizarAsync(usuario);

            return true;
        }

        public async Task<bool> DesativarAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID deve ser maior que zero", nameof(id));

            // Verificar se usuário existe
            var usuario = await _repository.BuscarPorIdAsync(id);
            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado");

            // Verificar se já está desativado
            if (!usuario.Ativo)
                throw new InvalidOperationException("Usuário já está desativado");

            // Regra de negócio: não permitir desativar o último admin
            if (usuario.TipoUsuario == TipoUsuario.Admin)
            {
                var adminsAtivos = await _repository.BuscarPorTipoAsync(TipoUsuario.Admin);
                if (adminsAtivos.Count() <= 1)
                    throw new InvalidOperationException("Não é possível desativar o último administrador do sistema");
            }

            return await _repository.DeletarAsync(id);
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return await _repository.EmailExisteAsync(email);
        }

        #region Métodos Privados de Validação

        private void ValidarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha é obrigatória");

            if (senha.Length < 8)
                throw new ArgumentException("Senha deve ter no mínimo 8 caracteres");

            if (senha.Length > 100)
                throw new ArgumentException("Senha muito longa (máximo 100 caracteres)");

            // Validações básicas de composição
            if (!PossuiCaracteresMaiscula(senha))
                throw new ArgumentException("Senha deve conter pelo menos uma letra maiúscula");

            if (!PossuiCaracteresMinuscula(senha))
                throw new ArgumentException("Senha deve conter pelo menos uma letra minúscula");

            if (!PossuiNumeros(senha))
                throw new ArgumentException("Senha deve conter pelo menos um número");

            // Apenas padrões óbvios - resto no frontend
            if (EhPadraoObvio(senha))
                throw new ArgumentException("Senha segue um padrão muito simples");
        }

        private void ValidarDadosAtualizacao(AtualizarUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("Nome é obrigatório");

            if (dto.Nome.Length < 2)
                throw new ArgumentException("Nome deve ter no mínimo 2 caracteres");

            if (dto.Nome.Length > 100)
                throw new ArgumentException("Nome muito longo (máximo 100 caracteres)");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email é obrigatório");

            if (!IsValidEmail(dto.Email))
                throw new ArgumentException("Email inválido");

            if (!string.IsNullOrEmpty(dto.Telefone) && dto.Telefone.Length > 20)
                throw new ArgumentException("Telefone muito longo (máximo 20 caracteres)");

            if (dto.DataNascimento.HasValue && dto.DataNascimento.Value > DateTime.Now)
                throw new ArgumentException("Data de nascimento não pode ser futura");
        }

        private bool PossuiCaracteresMaiscula(string senha) => senha.Any(char.IsUpper);
        private bool PossuiCaracteresMinuscula(string senha) => senha.Any(char.IsLower);
        private bool PossuiNumeros(string senha) => senha.Any(char.IsDigit);

        private bool EhPadraoObvio(string senha)
        {
            var senhaLower = senha.ToLower();

            // Sequências numéricas
            if (senhaLower.Contains("12345") || senhaLower.Contains("54321"))
                return true;

            // Sequências de teclado
            if (senhaLower.Contains("qwerty") || senhaLower.Contains("asdf"))
                return true;

            // Repetições
            if (senha.All(c => c == senha[0])) // "aaaaaaaa"
                return true;

            return false;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}