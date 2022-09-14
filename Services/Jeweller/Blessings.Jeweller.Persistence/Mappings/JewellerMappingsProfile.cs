using AutoMapper;
using Blessings.Jeweller.Core.CQRS;

namespace Blessings.Jeweller.Core.Mappings;

public class JewellerMappingsProfile : Profile
{
    public JewellerMappingsProfile()
    {
        CreateMap<Domain.Jeweller, CreateJewellerResponse>();
    }
}