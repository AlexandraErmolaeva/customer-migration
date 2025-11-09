namespace Application.Common.Dto;

public abstract record UpdateContactsCommandBase
{
    public string? PhoneMobile { get; set; }
    public string? Email { get; set; }
}
