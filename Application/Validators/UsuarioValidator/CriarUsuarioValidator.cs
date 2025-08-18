using Application.DTO.Usuario;
using FluentValidation;

namespace Application.Validators.UsuarioValidator
{
    public class CriarUsuarioValidator : AbstractValidator<CriarUsuarioDto>
    {
        public CriarUsuarioValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .Length(2, 100).WithMessage("Nome deve ter entre 2 e 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email inválido")
                .MaximumLength(255).WithMessage("Email muito longo");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres");

            RuleFor(x => x.TipoUsuario)
                .InclusiveBetween(1, 3).WithMessage("Tipo de usuário inválido");

            RuleFor(x => x.Telefone)
                .MaximumLength(20).WithMessage("Telefone muito longo")
                .When(x => !string.IsNullOrEmpty(x.Telefone));
        }
    }
}
