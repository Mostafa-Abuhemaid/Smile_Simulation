using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Smile_Simulation.Domain.DTOs.DoctorDto;
using Smile_Simulation.Domain.DTOs.PatientDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces.Services;
using Smile_Simulation.Domain.Response;
using Smile_Simulation.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly SmileDbContext _DbContext;
        public UserService(UserManager<UserApp> userManager, IConfiguration configuration, IMapper mapper, SmileDbContext dbContext)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _DbContext = dbContext;
        }

        public async Task<BaseResponse<SendDoctorDTO>> GetDoctorDetailsAsync(string DoctorId,string role)
        {
            
                var Doc = await _DbContext.Doctors.FirstOrDefaultAsync(d => d.Id == DoctorId);
                if (Doc == null)
                    return new BaseResponse<SendDoctorDTO>(false, "المستخدم غير موجود");

                var Url = $"{_configuration["BaseURL"]}/Images/Doctor/Profile{Doc.Image}";
                var DocDTO = new SendDoctorDTO
                {
                    Email = Doc.Email,
                    Image = Doc.Image != null ? $"{_configuration["BaseURL"]}/Images/Doctor/Profile/{Doc.Image}" : null,
                    FullName = Doc.FullName,
                    Gender = Doc.gender,
                    Experience = Doc.Experience,
                    Qualification = Doc.Qualification,
                    Specialization = Doc.Specialization,
                    Address = Doc.Address,
                    BirthDay = Doc.BirthDay,
                    role=role
                    
                };
              
                return new BaseResponse<SendDoctorDTO>(true, "تم الوصول الى بيانات الطبيب  ", DocDTO);

        }

        public async Task<BaseResponse<SendPatientDTO>> GetPatientDetailsAsync(string PatientId,string role)
        {
            var patient = await _DbContext.Patients.FirstOrDefaultAsync(d => d.Id == PatientId);
            if (patient == null)
                return new BaseResponse<SendPatientDTO>(false, "المستخدم غير موجود");
            
            var PatientDTO = new SendPatientDTO
            {
                Email = patient.Email,
                Image = patient.Image != null ? $"{_configuration["BaseURL"]}/Images/Patient/{patient.Image}" : null,
                FullName = patient.FullName,
                gender = patient.gender,
                  Age= patient.Age,
                Address = patient.Address,
                BirthDay = patient.BirthDay,
                role=role
            };

            return new BaseResponse<SendPatientDTO>(true, "تم الوصول الى بيانات المريض بنجاح ", PatientDTO);
        }
    }
}
