using System.Text.RegularExpressions;

namespace Infrastructure.Helpers;

public static class PhoneNumberHelper
{
    static string _defaultPattern = @"^79\d{9}$";

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
        => phoneNumber
            .Replace("+", "")
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");

    private static bool IsValidPhoneNumber(string normalizedPhoneNumber)
        => Regex.IsMatch(normalizedPhoneNumber, _defaultPattern);
}
