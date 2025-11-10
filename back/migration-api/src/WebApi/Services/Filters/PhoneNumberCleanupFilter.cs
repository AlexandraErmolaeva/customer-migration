using Application.Common.Validators;
using Application.UseCases.Commands.Update.Customer;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace WebApi.Services.Filters;

/// <summary>
/// Очищаем у входящего номера телефона (если это команда для обновления сущности) все символы, кроме цифр.
/// </summary>
public class PhoneNumberCleanupFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("command", out var commandTemp) && commandTemp is UpdateCustomerCommand command)
        {
            if (command.Contacts != null && !string.IsNullOrWhiteSpace(command.Contacts.PhoneMobile))
                command.Contacts.PhoneMobile = Regex.Replace(command.Contacts.PhoneMobile.Trim(), RegexPatterns.PHONE_SYMBOLS_CLEANUP_PATTERN, "");
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
