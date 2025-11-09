using Domain.Entities;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Infrastructure.Helpers;

public static class DataParsingHelper
{
    private const string NAME_DEFAULT_PATTERN = @"[^a-zA-Zа-яА-ЯёЁ\s]";
    private const string GENDER_DEFAULT_PATTERN = @"[^а-яё]";

    private static readonly Dictionary<string, Gender> _genderMap = new()
    {
        ["м"] = Gender.MALE,
        ["муж"] = Gender.MALE,
        ["мужской"] = Gender.MALE,

        ["ж"] = Gender.FEMALE,
        ["жен"] = Gender.FEMALE,
        ["женский"] = Gender.FEMALE
    };

    /// <summary>
    /// Распарсить строку как decimal, если значения нет - возвращаем 0.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal ParseDecimal(string value)
        => decimal.TryParse(value, out var decimalValue) ? decimalValue : 0;

    /// <summary>
    /// Распарсить номер телефона.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ParsePhoneMobile(string value)
        => PhoneNumberHelper.TryGetValidPhoneNumber(value);


    /// <summary>
    /// Парс гендера. В БД хранятся текстовые представления Enum значения.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Gender? ParseGender(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        value = value.Trim().ToLower();
        value = Regex.Replace(value, GENDER_DEFAULT_PATTERN, "");
        return _genderMap.TryGetValue(value, out var gender) ? gender : null;
    }

    /// <summary>
    /// Распарсить дату. Приходит с оффсетом. Учитываем точки в дате.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateOnly? ParseDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var spaceIndex = value.LastIndexOf(' ');
        var dateTimePart = spaceIndex > 0 ? value.Substring(0, spaceIndex) : value;

        if (DateTime.TryParse(dateTimePart, new CultureInfo("ru-RU"), DateTimeStyles.None, out var dateTime))
            return DateOnly.FromDateTime(dateTime);

        return null;
    }

    /// <summary>
    /// Оставляем имена в rus eng локализации, убираем символы и цифры.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string NormalizeName(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        value = value.Trim();
        var normalizedValue = Regex.Replace(value, NAME_DEFAULT_PATTERN, "");

        if (string.IsNullOrEmpty(normalizedValue))
            return null;

        return normalizedValue;
    }
}
