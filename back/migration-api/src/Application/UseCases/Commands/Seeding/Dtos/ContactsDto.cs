using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.Commands.Seeding.Dtos;

public class ContactsDto : IMapFrom<ContactsEntity>
{
    public Guid Id { get; set; }
    public string? PhoneMobile { get; set; }
    public string? Email { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ContactsEntity, ContactsDto>();
        profile.CreateMap<ContactsDto, ContactsEntity>();
    }
}
