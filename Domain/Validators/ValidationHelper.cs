using FluentValidation;

namespace Domain.Validators
{
    public class ValidationHelper
    {
        public static async Task ValidateAsync<T>(IValidator<T> validator, T instance, string ruleSet)
        {
            var result = await validator.ValidateAsync(instance, options => options.IncludeRuleSets(ruleSet));

            if (!result.IsValid)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
        }
    }
}
