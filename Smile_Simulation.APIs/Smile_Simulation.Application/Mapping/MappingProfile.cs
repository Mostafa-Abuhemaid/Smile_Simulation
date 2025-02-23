using AutoMapper;
using Smile_Simulation.Domain.DTOs.PatientDto;
using Smile_Simulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Application.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<PatientDto, Patient>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Image, opt => opt.Ignore()); ;
        }
    }
}
