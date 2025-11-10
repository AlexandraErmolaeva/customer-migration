using Application.Common.Validators;
using Application.UseCases.Commands.Update.Customer.Helpers.Contacts;
using Application.UseCases.Commands.Update.Customer.Helpers.FinancialProfile;
using FluentValidation;

namespace Application.UseCases.Commands.Update.Customer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.CardCode)
            .NotEmpty().WithMessage("код карты не может быть пустым");

        RuleFor(x => x.LastName)
            .Matches(RegexPatterns.NAME_DEFAULT_PATTERN)
            .WithMessage("в фамилии есть недопустимые символы")
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.FirstName)
            .Matches(RegexPatterns.NAME_DEFAULT_PATTERN)
            .WithMessage("В имени есть недопустимые символы.")
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.SurName)
            .Matches(RegexPatterns.NAME_DEFAULT_PATTERN)
            .WithMessage("в отчестве есть недопустимые символы")
            .When(x => !string.IsNullOrWhiteSpace(x.SurName));

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("недопустимое значение для genger`а")
            .When(x => x.Gender.HasValue);

        RuleFor(x => x.Birthday)
            .Must(IsBirthdayValid).WithMessage("дата рождения должна быть в прошлом")
            .When(x => x.Birthday.HasValue);

        RuleFor(x => x.City)
            .Matches(RegexPatterns.CITY_DEFAULT_PATTERN)
            .WithMessage("в названии города есть невалидные символы")
            .When(x => !string.IsNullOrWhiteSpace(x.City));

        RuleFor(x => x.Contacts)
            .NotNull()
            .SetValidator(new UpdateContactsCommandHelperValidator());

        RuleFor(x => x.FinancialProfile)
            .NotNull()
            .SetValidator(new UpdateFinancialProfileCommandHelperValidator());
    }

    private bool IsBirthdayValid(DateOnly? birthday)
    {
        return birthday.HasValue && birthday.Value < DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
