using Domain.Entities.General;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class FinancialProfileEntity : EntityBase
{
    public string? Pincode { get; private set; }
    public decimal Bonus { get; private set; }
    public decimal Turnover { get; private set; }

    [Required]
    public CustomerEntity Customer { get; private set; }
}
