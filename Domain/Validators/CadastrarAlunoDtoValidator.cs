

using Domain.DTOs;
using FluentValidation;

public class CadastrarAlunoDtoValidator : AbstractValidator<CadastrarAlunoDto>
{
    public CadastrarAlunoDtoValidator()
    {
        RuleSet("Cadastro", () =>
        {

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .Length(2, 100).WithMessage("Nome deve ter entre 2 e 100 caracteres")
                .Matches(@"^[a-zA-ZÀ-ÿ\u00C0-\u017F\s\-\.]+$")
                .WithMessage("Nome deve conter apenas letras, espaços, hífens e pontos");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter formato válido")
                .MaximumLength(150).WithMessage("Email deve ter no máximo 150 caracteres")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Email deve conter @ e domínio válido");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres")
                .MaximumLength(100).WithMessage("Senha deve ter no máximo 100 caracteres");

            RuleFor(x => x.Telefone)
                .Matches(@"^(\(\d{2}\)\s?|\d{2}\s?)?\d{4,5}-?\d{4}$")
                .WithMessage("Telefone deve ter formato válido: (XX) XXXXX-XXXX, XX XXXXX-XXXX ou XXXXXXXXXXX")
                .When(x => !string.IsNullOrEmpty(x.Telefone));

            RuleFor(x => x.DataNascimento)
                .LessThan(DateTime.Now.AddYears(-10))
                .WithMessage("Aluno deve ter pelo menos 10 anos")
                .GreaterThan(DateTime.Now.AddYears(-100))
                .WithMessage("Data de nascimento inválida")
                .When(x => x.DataNascimento.HasValue);

            RuleFor(x => x.Cidade)
                .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Cidade));

            RuleFor(x => x.AnoEscolar)
                .InclusiveBetween(1, 3).WithMessage("Ano escolar deve ser 1, 2 ou 3")
                .When(x => x.AnoEscolar.HasValue);
        });

        RuleSet("Basic", () =>
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .Length(2, 100).WithMessage("Nome deve ter entre 2 e 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha muito curta");
        });
    }
}