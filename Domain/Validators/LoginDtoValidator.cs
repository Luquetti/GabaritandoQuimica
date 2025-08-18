using Domain.DTOs;
using FluentValidation;

namespace Domain.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleSet("Login", () =>
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email é obrigatório")
                    .EmailAddress().WithMessage("Email deve ter formato válido")
                    .MaximumLength(150).WithMessage("Email deve ter no máximo 150 caracteres");

                RuleFor(x => x.Senha)
                    .NotEmpty().WithMessage("Senha é obrigatória")
                    .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres");
            });

            // RuleSet para validação simples (sem regras complexas)
            RuleSet("Basic", () =>
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email é obrigatório")
                    .EmailAddress().WithMessage("Email inválido");

                RuleFor(x => x.Senha)
                    .NotEmpty().WithMessage("Senha é obrigatória");
            });
        }
    }
}

