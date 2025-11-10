namespace Application.Common.Validators;

public static class RegexPatterns
{
    /// <summary>
    /// Пропускает русские и английские буквы.
    /// </summary>
    public const string NAME_DEFAULT_PATTERN = @"^[a-zA-Zа-яА-ЯёЁ]+$";

    /// <summary>
    /// Пропускает цифры от 0 до 9.
    /// </summary>
    public const string PHONE_DEFAULT_PATTERN = @"^79\d{9}$";

    /// <summary>
    /// Пропускает русские буквы, дефисы, тире и пробелы.
    /// </summary>
    public const string CITY_DEFAULT_PATTERN = @"^[а-яА-ЯёЁ\s\-\u2013]+$";

    /// <summary>
    /// Соответствует всем символам, кроме цифр, для очистки.
    /// </summary>
    public const string PHONE_CLEANAUP_PATTERN = @"[^\d]";

    /// <summary>
    /// Соответствуем всем символам, кроме () и +, для очистки.
    /// </summary>
    public const string PHONE_SYMBOLS_CLEANUP_PATTERN = @"[+()]";

    /// <summary>
    /// Соответствует всем символам, кроме русских и английских букв и пробелов, для очистки.
    /// </summary>
    public const string NAME_EXCLUDE_PATTERN = @"[^a-zA-Zа-яА-ЯёЁ\s]";

    /// <summary>
    /// Соответствует всем символам, кроме русских букв, для очистки.
    /// </summary>
    public const string GENDER_EXCLUDE_PATTERN = @"[^а-яА-ЯёЁ]";

    /// <summary>
    /// Соответствует всем символам, кроме русских букв, пробелов, дефисов и тире, для очистки.
    /// </summary>
    public const string CITY_EXCLUDE_PATTERN = @"[^а-яА-ЯёЁ\s\-\u2013]";
}
