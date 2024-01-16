using AutoMapper;
using PetService.Domain;


namespace PetService.Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<MissingPet, MissingPet>();
        }

    }
}
