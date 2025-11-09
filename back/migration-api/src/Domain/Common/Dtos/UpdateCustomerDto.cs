using Domain.Entities;

namespace Domain.Common.Dtos;

public class UpdateCustomerDto
{
    public string CardCode { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? SurName { get; set; }
    public Gender? Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? City { get; set; }
    public UpdateContactsDto UpdateContactsDto { get; set; }
    public UpdateFinancialProfileDto UpdateFinancialProfileDto { get; set; }
}
