using Domain.Entities;

namespace Application.Common.Dto;

public abstract record UpdateCustomerCommandBase
{
    public string CardCode { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? SurName { get; set; }
    public Gender? Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? City { get; set; }
}
