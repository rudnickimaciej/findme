using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PetService.Application.Dtos;
using PetService.Domain;

namespace PetService.Application.Profiles
{
    public class MissingPetProfile : Profile
    {
        public MissingPetProfile()
        {
            CreateMap<ReportMissingPetDto, AddMissingPetDto>()
                .ForMember(dest => dest.PhotoUrls, opt => opt.Ignore());

            CreateMap<AddMissingPetDto, MissingPet>();
            CreateMap<MissingPet, ReportMissingPetDto>();

        }
    }
}