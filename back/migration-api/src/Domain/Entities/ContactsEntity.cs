using Domain.Common.Dtos;
using Domain.Entities.General;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ContactsEntity : EntityBase
{
    public string? PhoneMobile { get; private set; }
    public string? Email { get; private set; }

    [Required]
    public CustomerEntity Customer { get; private set; }

    public ContactsEntity Update(UpdateContactsDto dto)
    {
        LastModifiedAt = DateTime.Now;
        PhoneMobile = dto.PhoneMobile;
        Email = dto.Email;

        return this;
    }
}
