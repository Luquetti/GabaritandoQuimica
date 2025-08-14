using Application.DTO.Serie;
using FluentValidation;

namespace Application.Validators.SerieValidator
{
    public class CriarSerieValidator : AbstractValidator<CriarSerieDto>
    {
        public CriarSerieValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .Length(2, 50).WithMessage("Nome deve ter entre 2 e 50 caracteres");

            RuleFor(x => x.Preco)
                .GreaterThan(0).WithMessage("Preço deve ser maior que zero");

            RuleFor(x => x.Ordem)
                .GreaterThan(0).WithMessage("Ordem deve ser maior que zero");

            RuleFor(x => x.Descricao)
                .MaximumLength(500).WithMessage("Descrição muito longa")
                .When(x => !string.IsNullOrEmpty(x.Descricao));
        }
    }
}
