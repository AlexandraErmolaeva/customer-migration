using Application.Common.Validators;
using Domain.Entities;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Infrastructure.Helpers;

public static class RowDataParsingHelper
{
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
        var stringGender = NormalizeByPattern(RegexPatterns.GENDER_EXCLUDE_PATTERN, value);
        if (string.IsNullOrWhiteSpace(stringGender))
            return null;

        stringGender = stringGender.ToLower();

        return _genderMap.TryGetValue(stringGender, out var gender) ? gender : null;
    }

    /// <summary>
    /// Распарсить дату. Приходит с оффсетом. Учитываем точки в дате.
    /// По формуле из ексель дата может прийти как 0.12.1996.
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
        var name = NormalizeByPattern(RegexPatterns.NAME_EXCLUDE_PATTERN, value);
        return name;
    }

    /// <summary>
    /// Парсим город. Оставляем тире, пробелы.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string NormalizeCity(string value)
    {
        var city = NormalizeByPattern(RegexPatterns.CITY_EXCLUDE_PATTERN, value);
        return city;
    }

    private static string NormalizeByPattern(string pattern, string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        value = value.Trim();
        var normalizedValue = Regex.Replace(value, pattern, "");

        if (string.IsNullOrEmpty(normalizedValue))
            return null;

        return normalizedValue;
    }
}
