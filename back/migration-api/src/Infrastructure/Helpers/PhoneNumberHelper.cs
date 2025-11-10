using Application.Common.Dto;
using Application.Common.Validators;
using System.Text.RegularExpressions;

namespace Infrastructure.Helpers;

public static class PhoneNumberHelper
{
    /// <summary>
    /// Возвращает или валидный номер телефона ="+7"&СЛУЧМЕЖДУ(9000000000;9999999999) без "+", или null.
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public static string TryGetValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return null;

        var normalizedPhoneNumber = NormalizePhoneNumber(phoneNumber);

        if (IsValidPhoneNumber(normalizedPhoneNumber))
            return normalizedPhoneNumber;
        else
            return null;
    }

    private static string NormalizePhoneNumber(string phoneNumber)
        => phoneNumber = Regex.Replace(phoneNumber, RegexPatterns.PHONE_CLEANAUP_PATTERN, "");

    private static bool IsValidPhoneNumber(string normalizedPhoneNumber)
        => Regex.IsMatch(normalizedPhoneNumber, RegexPatterns.PHONE_DEFAULT_PATTERN);
}
