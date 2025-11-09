using Application.Common.Dto;
using Application.UseCases.Commands.Seeding.Dtos;
using Application.UseCases.Commands.Update.Customer.Helpers.Contacts;
using Application.UseCases.Commands.Update.Customer.Helpers.FinancialProfile;
using MediatR;

namespace Application.UseCases.Commands.Update.Customer;

public record UpdateCustomerCommand : UpdateCustomerCommandBase, IRequest<Result<CustomerDto>>
{
    public required Guid Id { get; set; }
    public required UpdateContactsCommandHelper Contacts {get; set;}
    public required UpdateFinancialProfileCommandHelper FinancialProfile { get; set;}
}
