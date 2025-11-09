using Application.Common.Dto;
using Application.Dependencies.DataAccess;
using Application.Dependencies.Logging;
using Application.UseCases.Commands.Seeding.Dtos;
using AutoMapper;
using Domain.Common.Dtos;
using MediatR;

namespace Application.UseCases.Commands.Update.Customer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public UpdateCustomerCommandHandler(IUnitOfWork unitOFWork, IMapper mapper, ILoggerManager logger)
    {
        _unitOfWork = unitOFWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entityToUpdate = await _unitOfWork.Customers
            .GetFilteredWithIncludes(entity => entity.Id == request.Id, readOnly: false, includes: [entity => entity.Contacts, entity => entity.FinancialProfile])
            ?? throw new Exception($"Клиент с ID {request.Id} не найден.");

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            entityToUpdate.Update(CreateDomainDto(request));
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError($"Произошла ошибка при обновлении Customer агрегата: {ex}.");
            throw;
        }

        var dto = _mapper.Map<CustomerDto>(entityToUpdate);
        return Result<CustomerDto>.Success(dto);
    }

    private UpdateCustomerDto CreateDomainDto(UpdateCustomerCommand command)
    {
        return new UpdateCustomerDto
        {
            Birthday = command.Birthday,
            SurName = command.SurName,
            CardCode = command.CardCode,
            City = command.City,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Gender = command.Gender,

            UpdateContactsDto = new UpdateContactsDto
            {
                Email = command.Contacts.Email,
                PhoneMobile = command.Contacts.PhoneMobile
            },

            UpdateFinancialProfileDto = new UpdateFinancialProfileDto
            {
                Bonus = command.FinancialProfile.Bonus,
                Pincode = command.FinancialProfile.Pincode,
                Turnover = command.FinancialProfile.Turnover,
            }
        };
    }
}
