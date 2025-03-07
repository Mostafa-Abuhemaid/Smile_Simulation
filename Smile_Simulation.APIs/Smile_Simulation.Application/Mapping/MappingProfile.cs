using AutoMapper;
using Smile_Simulation.Domain.DTOs.DoctorDto;
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
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
              .ForMember(dest => dest.gender, opt => opt.MapFrom(src => src.gender))
              .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<DoctorDto, Doctor>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.Image, opt => opt.Ignore())  
                 .ForMember(dest => dest.Card, opt => opt.Ignore())
                  .ForMember(dest => dest.Qualification, opt => opt.MapFrom(src => src.Qualification ?? string.Empty)) 
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization ?? string.Empty))
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience ?? 0));
        }
    }
}
