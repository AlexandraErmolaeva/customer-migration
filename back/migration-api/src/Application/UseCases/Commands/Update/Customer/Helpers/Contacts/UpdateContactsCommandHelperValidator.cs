using Application.Common.Validators;
using FluentValidation;

namespace Application.UseCases.Commands.Update.Customer.Helpers.Contacts;

public class UpdateContactsCommandHelperValidator : AbstractValidator<UpdateContactsCommandHelper>
{
    public UpdateContactsCommandHelperValidator()
    {
        RuleFor(x => x.PhoneMobile)
            .Matches(RegexPatterns.PHONE_DEFAULT_PATTERN)
            .WithMessage("номер телефона невалиден")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneMobile));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("некорректный формат email")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
