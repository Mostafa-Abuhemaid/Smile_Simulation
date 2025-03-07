using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Smile_Simulation.Domain.DTOs.AccountDto;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Smile_Simulation.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        public AccountService(UserManager<UserApp> userManager, SignInManager<UserApp> signInManager, IConfiguration configuration, ITokenService tokenService, IMapper mapper, IMemoryCache memoryCache, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _emailService = emailService;
        }

        public async Task<ForgotPasswordDTO> ForgotPasswordAsync(ForgotDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("لم يتم العثور على بريدك الإلكتروني");
            }

            var otp = new Random().Next(100000, 999999).ToString();
            _memoryCache.Set(request.Email, otp, TimeSpan.FromMinutes(60));
            await _emailService.SendEmailAsync(request.Email, "Smile-Simulation", $"Your VerifyOTP code is: {otp}");

            return new ForgotPasswordDTO
            {
                Token = await _userManager.GeneratePasswordResetTokenAsync(user),
                
            };
        }

        public async Task<TokenDTO> LoginAsync(LoginDto LoginDto)
        {
            var user = await _userManager.FindByEmailAsync(LoginDto.Email);
            if (user == null) throw new Exception("البريد الاكتروني غير صحيح ");

            var result = await _signInManager.CheckPasswordSignInAsync(user, LoginDto.Password, false);
            if (!result.Succeeded) throw new Exception("كلمة السر غير صحيحة");
            var Url = $"{_configuration["BaseURL"]}/Images/Product/{user.Image}";
            return new TokenDTO
            {
                Email = user.Email,
                FullName= user.FullName,
                gender=user.gender,
                Image= Url,
                Token = await _tokenService.GenerateTokenAsync(user, _userManager)
            };
        }

        public async Task<TokenForRegister> RegisterForDoctorAsync(DoctorDto doctorDto)
        {
            if (doctorDto.Password != doctorDto.ConfirmPassword)
                throw new Exception("كلمة المرور وتأكيد كلمة المرور لا يتطابقان");

            if (!new EmailAddressAttribute().IsValid(doctorDto.Email))
                throw new Exception("تنسيق البريد الإلكتروني غير صالح");


            var existingUser = await _userManager.FindByEmailAsync(doctorDto.Email);
            if (existingUser != null)
                throw new Exception("يوجد مستخدم لديه هذا البريد الإلكتروني بالفعل.");

            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(doctorDto.Password, passwordPattern))
            {
                throw new Exception("كلمة المرور يجب أن تحتوي على أحرف كبيرة وصغيرة وأرقام ورموز.");
            }


            if (doctorDto.Correct == false)
                throw new Exception("صورة الكارنية غير صحيحة");

            var doctor = new Doctor
            {
                FullName=doctorDto.FullName,
                UserName=doctorDto.Email,
                Email=doctorDto.Email,
                gender = doctorDto.Gender,
                Experience= doctorDto.Experience,
                Specialization= doctorDto.Specialization,
                Qualification= doctorDto.Qualification,
            };

          
            doctor.Image = Files.UploadFile(doctorDto.Image, "Doctor\\Profile");
            doctor.Card = Files.UploadFile(doctorDto.Card, "Doctor\\Card");
           
            var result = await _userManager.CreateAsync(doctor, doctorDto.Password);

            if (!result.Succeeded)
                throw new Exception("فشل إنشاء الحساب");


            await _userManager.AddToRoleAsync(doctor, Roles.Doctor.ToString());

            return new TokenForRegister
            {
                Email = doctorDto.Email,

                Token = await _tokenService.GenerateTokenAsync(doctor, _userManager)
            };
           
        }

        public async Task<TokenForRegister> RegisterForPatientAsync(PatientDto patientDto)
        {
    

            if (patientDto.Password != patientDto.ConfirmPassword)
                throw new Exception("كلمة المرور وتأكيد كلمة المرور لا يتطابقان");

            if (!new EmailAddressAttribute().IsValid(patientDto.Email))
                throw new Exception("تنسيق البريد الإلكتروني غير صالح");

            var existingUser = await _userManager.FindByEmailAsync(patientDto.Email);
            if (existingUser != null)
                throw new Exception("يوجد مستخدم لديه هذا البريد الإلكتروني بالفعل");

            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(patientDto.Password, passwordPattern))
            {
                throw new Exception("كلمة المرور يجب أن تحتوي على أحرف كبيرة وصغيرة وأرقام ورموز");
            }


            var patient = new Patient
            { 
                UserName=patientDto.Email,
                FullName= patientDto.FullName,
                Email = patientDto.Email,
                Age = patientDto.Age,
                gender=patientDto.gender,

            };
            patient.Image = Files.UploadFile(patientDto.Image, "Patient");
       
            var result = await _userManager.CreateAsync(patient, patientDto.Password);

            if (!result.Succeeded)
                throw new Exception("فشل إنشاء الحساب");

            await _userManager.AddToRoleAsync(patient,Roles.Patient.ToString());

            return new TokenForRegister
            {
                Email = patientDto.Email,
                Token = await _tokenService.GenerateTokenAsync(patient, _userManager)
            };
        
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            if (resetPassword.NewPassword != resetPassword.ConfirmNewPassword)
                throw new Exception("كلمة المرور وتأكيد كلمة المرور لا يتطابقان");

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null) throw new Exception("لم يتم العثور على بريدك الإلكتروني");

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (!result.Succeeded) throw new Exception("فشلت إعادة تعيين كلمة المرور");

            return true;
        }

        public async Task<bool> VerifyOTPAsync(VerifyCodeDto verify)
        {
            var user = await _userManager.FindByEmailAsync(verify.Email);
            if (user == null) throw new Exception($"Email '{verify.Email}' is not found.");

            var cachedOtp = _memoryCache.Get(verify.Email)?.ToString();
            if (string.IsNullOrEmpty(cachedOtp)) throw new Exception("لم يتم العثور على الرمز أو انتهت صلاحيته.");

            if (verify.CodeOTP != cachedOtp) throw new Exception("الرمز غير صحيح");

            return true;
        
          }
    }
}
