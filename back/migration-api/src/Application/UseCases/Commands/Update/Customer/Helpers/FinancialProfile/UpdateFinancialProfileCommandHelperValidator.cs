using FluentValidation;

namespace Application.UseCases.Commands.Update.Customer.Helpers.FinancialProfile;

public class UpdateFinancialProfileCommandHelperValidator : AbstractValidator<UpdateFinancialProfileCommandHelper>
{
    public UpdateFinancialProfileCommandHelperValidator()
    {
        RuleFor(x => x.Bonus)
            .GreaterThanOrEqualTo(0).WithMessage("бонус не может быть отрицательным");
    }
}