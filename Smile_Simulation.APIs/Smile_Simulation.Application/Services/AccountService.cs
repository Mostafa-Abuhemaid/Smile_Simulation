using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Smile_Simulation.Domain.DTOs.DoctorDto;
using Smile_Simulation.Domain.DTOs.PatientDto;
using Smile_Simulation.Domain.DTOs.TokenDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Enums;
using Smile_Simulation.Domain.Interfaces.Services;
using Smile_Simulation.Infrastructure.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountService(UserManager<UserApp> userManager, SignInManager<UserApp> signInManager, IConfiguration configuration, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<TokenDTO> RegisterForDoctorAsync(DoctorDto doctorDto)
        {
            if (doctorDto.Password != doctorDto.ConfirmPassword)
                throw new Exception("Password and Confirm Password do not match");

            if (!new EmailAddressAttribute().IsValid(doctorDto.Email))
                throw new Exception("Invalid email format.");

            var existingUser = await _userManager.FindByEmailAsync(doctorDto.Email);
            if (existingUser != null)
                throw new Exception("A user with this email already exists.");

            var ImagePath = Files.UploadFile(doctorDto.Image, "Patient");

            var patient = _mapper.Map<Patient>(doctorDto);
            patient.Image = ImagePath;
            var result = await _userManager.CreateAsync(patient, doctorDto.Password);

            if (!result.Succeeded)
                throw new Exception("patient creation failed");

            await _userManager.AddToRoleAsync(patient, Roles.Doctor.ToString());

            var res = new TokenDTO
            {
                Email = doctorDto.Email,
                Token = await _tokenService.GenerateTokenAsync(patient, _userManager)
            };
            return res;
        }

        public async Task<TokenDTO> RegisterForPatientAsync(PatientDto patientDto)
        {
    

            if (patientDto.Password != patientDto.ConfirmPassword)
                throw new Exception("Password and Confirm Password do not match");

            if (!new EmailAddressAttribute().IsValid(patientDto.Email))
                throw new Exception("Invalid email format.");

            var existingUser = await _userManager.FindByEmailAsync(patientDto.Email);
            if (existingUser != null)
                throw new Exception("A user with this email already exists.");

            var ImagePath = Files.UploadFile(patientDto.Image, "Patient");

            var patient = _mapper.Map<Patient>(patientDto);
            patient.Image = ImagePath;
            var result = await _userManager.CreateAsync(patient, patientDto.Password);

            if (!result.Succeeded)
                throw new Exception("patient creation failed");

            await _userManager.AddToRoleAsync(patient,Roles.Patient.ToString());

            var res = new TokenDTO
            {
                Email = patientDto.Email,
                Token = await _tokenService.GenerateTokenAsync(patient, _userManager)
            };
            return res;
        }
    }
}
