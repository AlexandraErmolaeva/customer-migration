using Application.Common.Dto;

namespace Application.UseCases.Commands.Update.Customer.Helpers.Contacts;

public record UpdateContactsCommandHelper : UpdateContactsCommandBase
{
    public Guid Id { get; set; }
}
