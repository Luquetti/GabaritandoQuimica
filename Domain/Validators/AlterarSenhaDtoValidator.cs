using Domain.DTOs;
using FluentValidation;

namespace Domain.Validators
{
    public class AlterarSenhaDtoValidator : AbstractValidator<AlterarSenhaDto>
    {
        public AlterarSenhaDtoValidator()
        {
            RuleSet("AlterarSenha", () =>
            {
                RuleFor(x => x.SenhaAtual)
                    .NotEmpty().WithMessage("Senha atual é obrigatória");

                RuleFor(x => x.NovaSenha)
                    .NotEmpty().WithMessage("Nova senha é obrigatória")
                    .MinimumLength(6).WithMessage("Nova senha deve ter pelo menos 6 caracteres")
                    .MaximumLength(100).WithMessage("Nova senha deve ter no máximo 100 caracteres")
                    .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$")
                    .WithMessage("Nova senha deve conter pelo menos: 1 letra minúscula, 1 maiúscula e 1 número");

                RuleFor(x => x.NovaSenha)
                    .NotEqual(x => x.SenhaAtual)
                    .WithMessage("Nova senha deve ser diferente da senha atual");
            });

            RuleSet("Basic", () =>
            {
                RuleFor(x => x.SenhaAtual)
                    .NotEmpty().WithMessage("Senha atual é obrigatória");

                RuleFor(x => x.NovaSenha)
                    .NotEmpty().WithMessage("Nova senha é obrigatória")
                    .MinimumLength(6).WithMessage("Nova senha muito curta");
            });

            RuleSet("ResetSenha", () =>
            {
                RuleFor(x => x.NovaSenha)
                    .NotEmpty().WithMessage("Nova senha é obrigatória")
                    .MinimumLength(6).WithMessage("Nova senha deve ter pelo menos 6 caracteres")
                    .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$")
                    .WithMessage("Nova senha deve conter pelo menos: 1 letra minúscula, 1 maiúscula e 1 número");
            });
        }
    }
}
