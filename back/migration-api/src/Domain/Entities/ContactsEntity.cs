using Domain.Entities.General;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ContactsEntity : EntityBase
{
    public string? PhoneMobile { get; private set; }
    public string? Email { get; private set; }

    [Required]
    public CustomerEntity Customer { get; private set; }
}
