using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Smile_Simulation.Domain.DTOs.DoctorDto;
using Smile_Simulation.Domain.DTOs.PatientDto;
using Smile_Simulation.Domain.DTOs.UserDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Enums;
using Smile_Simulation.Domain.Interfaces.Services;
using Smile_Simulation.Domain.Response;
using Smile_Simulation.Infrastructure.Data;
using Smile_Simulation.Infrastructure.Files;
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

        public async Task<BaseResponse<SendDoctorDTO>> EditDoctorDetailsAsync(string userId,EditeUserDto userDto)
        {
            var Doc = await _DbContext.Doctors.FirstOrDefaultAsync(d => d.Id == userId);
            if (Doc == null)
                return new BaseResponse<SendDoctorDTO>(false, "المستخدم غير موجود");

            if (userDto.Image != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Doc.Image))
                    {
                        Files.DeleteFile(Doc.Image, "Doctor\\Profile");
                    }
                    Doc.Image = Files.UploadFile(userDto.Image, "Doctor\\Profile");
                }
                catch (Exception ex)
                {
                   
                    return new BaseResponse<SendDoctorDTO>(false, "حدث خطأ أثناء معالجة الصورة");
                }
            }

            Doc.FullName = userDto.FullName;
            Doc.gender = userDto.Gender;
            Doc.Experience = userDto.Experience;
            Doc.Qualification = userDto.Qualification;
            Doc.Specialization = userDto.Specialization;
            Doc.Address = userDto.Address;
            Doc.BirthDay = userDto.BirthDay;
            await _DbContext.SaveChangesAsync();

            return new BaseResponse<SendDoctorDTO>(true, "تم تعديل البيانات بنجاح" );

        }

        public async Task<BaseResponse<SendPatientDTO>> EditPatientDetailsAsync(string userId, EditeUserDto userDto)
        {
            var patient = await _DbContext.Patients.FirstOrDefaultAsync(d => d.Id == userId);
            if (patient == null)
                return new BaseResponse<SendPatientDTO>(false, "المستخدم غير موجود");
          
            if (userDto.Image != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(patient.Image))
                    {
                        Files.DeleteFile(patient.Image, "Patient");
                    }
                    patient.Image = Files.UploadFile(userDto.Image, "Patient");
                }
                catch (Exception ex)
                {
                   
                    return new BaseResponse<SendPatientDTO>(false, "حدث خطأ أثناء معالجة الصورة");
                }
            }
                patient.FullName = userDto.FullName;
           patient.gender = userDto.Gender;
           patient.Age = userDto.age??0;
           patient.Address = userDto.Address;
           patient.BirthDay = userDto.BirthDay;
            await _DbContext.SaveChangesAsync();

            return new BaseResponse<SendPatientDTO>(true, "  تم تعديل البيانات بنجاح");
        }

        public async Task<BaseResponse<bool>> EditUserImagesAsync(string userId, EditUserImageDto file)
        {
           var User =await _userManager.FindByIdAsync(userId);
           
           string imageUrl;
            if(User == null)
                return new BaseResponse<bool>(false, "المستخدم غير موجود");
             var roles = await _userManager.GetRolesAsync(User);
            if (User.Image != null)
            {
                if (roles.Contains("Doctor"))
                {
                    Files.DeleteFile(User.Image, "Doctor\\Profile");
                    User.Image = Files.UploadFile(file.Image, "Doctor\\Profile");
                }
                else
                {
                    Files.DeleteFile(User.Image, "Patient");
                    User.Image = Files.UploadFile(file.Image, "Patient");
                }
                await _DbContext.SaveChangesAsync();
                return new BaseResponse<bool>(true, "تم تعديل البيانات بنجاح");
            }
            else
            {
                if (roles.Contains("Doctor"))
                {
                   
                    User.Image = Files.UploadFile(file.Image, "Doctor\\Profile");
                }
                else
                {
                  
                    User.Image = Files.UploadFile(file.Image, "Patient");
                }
                await _DbContext.SaveChangesAsync();
                return new BaseResponse<bool>(true, "تم تعديل البيانات بنجاح");
            }
            
            
          
        }

        public async Task<BaseResponse<SendDoctorDTO>> GetDoctorDetailsAsync(string DoctorId,string role)
        {
            
                var Doc = await _DbContext.Doctors.FirstOrDefaultAsync(d => d.Id == DoctorId);
                if (Doc == null)
                    return new BaseResponse<SendDoctorDTO>(false, "المستخدم غير موجود");
            var Url = $"{_configuration["BaseURL"]}/Images/Doctor/Profile/{Doc.Image}";
                var DocDTO = new SendDoctorDTO
                {
                    Id=DoctorId,
                    Email = Doc.Email,
                    Image = Doc.Image != null ? $"{_configuration["BaseURL"]}/Doctor/Profile/{Doc.Image}" : null,
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
                Id= PatientId,
                Email = patient.Email,
                Image = patient.Image != null ? $"{_configuration["BaseURL"]}/Patient/{patient.Image}" : null,
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
