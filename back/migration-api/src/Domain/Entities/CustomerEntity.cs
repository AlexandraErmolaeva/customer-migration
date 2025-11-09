using Domain.Common.Dtos;
using Domain.Entities.General;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class CustomerEntity : EntityBase
{
    [Required]
    public string CardCode { get; private set; }
    public string? LastName { get; private set; }
    public string? FirstName { get; private set; }
    public string? SurName { get; private set; }
    public Gender? Gender { get; private set; }
    public DateOnly? Birthday { get; private set; }
    public string? City { get; private set; }

    public ContactsEntity Contacts { get; private set; }
    public Guid? ContactsId { get; private set; }

    public FinancialProfileEntity FinancialProfile { get; private set; }
    public Guid? FinancialProfileId { get; private set; }

    public CustomerEntity Update(UpdateCustomerDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CardCode))
            throw new ArgumentException("CardCode не может быть пустым.");

        LastModifiedAt = DateTime.UtcNow;
        CardCode = dto.CardCode;
        LastName = dto.LastName;
        FirstName = dto.FirstName;
        SurName = dto.SurName;
        Gender = dto.Gender;
        Birthday = dto.Birthday;
        City = dto.City;

        Contacts.Update(dto.UpdateContactsDto);
        FinancialProfile.Update(dto.UpdateFinancialProfileDto);

        return this;
    }
}

public enum Gender
{
    FEMALE = 2,
    MALE = 4
}
