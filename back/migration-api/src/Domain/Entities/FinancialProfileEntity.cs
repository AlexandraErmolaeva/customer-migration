using Domain.Common.Dtos;
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

    internal FinancialProfileEntity Update(UpdateFinancialProfileDto dto)
    {
        if (dto.Bonus < 0)
            throw new ArgumentException($"Невозможно установить отрицательное значение для бонуса.");

        LastModifiedAt = DateTime.UtcNow;
        Pincode = dto.Pincode;
        Bonus = dto.Bonus;
        Turnover = dto.Turnover;

        return this;
    }
}

