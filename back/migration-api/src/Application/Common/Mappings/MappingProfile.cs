using AutoMapper;
using System.Reflection;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
