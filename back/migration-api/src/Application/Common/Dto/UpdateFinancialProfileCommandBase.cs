namespace Application.Common.Dto;

public abstract record UpdateFinancialProfileCommandBase
{
    public string? Pincode { get; private set; }
    public decimal Bonus { get; private set; }
    public decimal Turnover { get; private set; }
}
