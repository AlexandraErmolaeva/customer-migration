namespace Application.Common.Dto;

public abstract record UpdateFinancialProfileCommandBase
{
    public string? Pincode { get; set; }
    public decimal Bonus { get; set; }
    public decimal Turnover { get; set; }
}
