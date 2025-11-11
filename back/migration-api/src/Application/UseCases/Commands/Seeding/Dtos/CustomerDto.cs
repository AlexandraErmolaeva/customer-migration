using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.Commands.Seeding.Dtos;

public class CustomerDto : IMapFrom<CustomerEntity>
{
    public Guid Id { get; set; }
    public string CardCode { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? SurName { get; set; }
    public Gender? Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? City { get; set; }

    public ContactsDto Contacts { get; set; }
    public FinancialProfileDto FinancialProfile { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CustomerEntity, CustomerDto>()
            .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Contacts != null ? src.Contacts : null))
            .ForMember(dest => dest.FinancialProfile, opt => opt.MapFrom(src => src.FinancialProfile != null ? src.FinancialProfile : null))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        profile.CreateMap<CustomerDto, CustomerEntity>()
            .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Contacts != null ? src.Contacts : null))
            .ForMember(dest => dest.FinancialProfile, opt => opt.MapFrom(src => src.FinancialProfile != null ? src.FinancialProfile : null))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}
