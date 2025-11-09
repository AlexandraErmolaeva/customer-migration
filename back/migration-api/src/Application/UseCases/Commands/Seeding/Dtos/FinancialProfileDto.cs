using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.Commands.Seeding.Dtos;

public class FinancialProfileDto : IMapFrom<FinancialProfileEntity>
{
    public Guid Id { get; set; }
    public string? Pincode { get; set; }
    public decimal Bonus { get; set; }
    public decimal Turnover { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<FinancialProfileEntity, FinancialProfileDto>();
        profile.CreateMap<FinancialProfileDto, FinancialProfileEntity>();
    }
}
