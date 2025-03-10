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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserService(UserManager<UserApp> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<SendDoctorDTO> GetDoctorDetailsAsync(string DoctorId)
        {
            var Doc= await _userManager.Users.OfType<Doctor>().FirstOrDefaultAsync(u => u.Id == DoctorId);
            if (Doc == null)
                throw new Exception("المستخدم غير موجود");

            //var Url = $"{_configuration["BaseURL"]}/Images//{user.Image}";
           
            var DocDTO= _mapper.Map<SendDoctorDTO>(Doc);
            //  DocDTO.Image= Url;
            return DocDTO;
        }

        public Task<SendPatientDTO> GetPatientDetailsAsync(string PatientId)
        {
            throw new NotImplementedException();
        }
    }
}
