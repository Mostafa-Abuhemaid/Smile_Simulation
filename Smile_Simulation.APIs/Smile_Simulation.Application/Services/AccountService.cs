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
using Smile_Simulation.Domain.Response;
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

        public async Task<BaseResponse<ForgotPasswordDTO>> ForgotPasswordAsync(ForgotDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new BaseResponse<ForgotPasswordDTO>(false, "لم يتم العثور على بريدك الإلكتروني");

            var otp = new Random().Next(100000, 999999).ToString();
            _memoryCache.Set(request.Email, otp, TimeSpan.FromMinutes(60));
            await _emailService.SendEmailAsync(request.Email, "Smile-Simulation", $"Your VerifyOTP code is: {otp}");

            var res= new ForgotPasswordDTO
            {
                Token = await _userManager.GeneratePasswordResetTokenAsync(user),

            };
          return  new BaseResponse<ForgotPasswordDTO>(true, "تحقق من بريدك الاكتروني", res);
        }

        public async Task<BaseResponse<TokenDTO>> LoginAsync(LoginDto loginDto)
        {
            if (loginDto == null)
                return new BaseResponse<TokenDTO>(false, "بيانات الدخول مطلوبة");

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null) return new BaseResponse<TokenDTO>(false, "البريد الاكتروني غير صحيح");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (!result.Succeeded) return new BaseResponse<TokenDTO>(false, "كلمة السر غير صحيحة");
            var roles = await _userManager.GetRolesAsync(user);
            string imageUrl;
            if (roles.Contains("Doctor")) 
            {
                imageUrl = $"{_configuration["BaseURL"]}/Doctor/Profile/{user.Image}";
            }
            else 
            {
                imageUrl = $"{_configuration["BaseURL"]}/Patient/{user.Image}";
            }
            var res = new TokenDTO
                {
                    UserId=user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    gender = user.gender,
                    Image = imageUrl,
                    Token = await _tokenService.GenerateTokenAsync(user, _userManager)
                };

                return new BaseResponse<TokenDTO>(true, "تم تسجيل الدخول بنجاح", res);
          
        }

        public async Task<BaseResponse<TokenForRegister>> RegisterForDoctorAsync(GetDoctorDto doctorDto)
        {
            if (doctorDto.Password != doctorDto.ConfirmPassword)
              return new BaseResponse<TokenForRegister>(false,"كلمة المرور وتأكيد كلمة المرور لا يتطابقان");

            if (!new EmailAddressAttribute().IsValid(doctorDto.Email))
                return new BaseResponse<TokenForRegister>(false,"تنسيق البريد الإلكتروني غير صالح");


            var existingUser = await _userManager.FindByEmailAsync(doctorDto.Email);
            if (existingUser != null)
                return new BaseResponse<TokenForRegister>(false,"يوجد مستخدم لديه هذا البريد الإلكتروني بالفعل.");


            var passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            if (!Regex.IsMatch(doctorDto.Password, passwordPattern))
            {
                return new BaseResponse<TokenForRegister>(false, "كلمة المرور يجب أن تحتوي على أحرف كبيرة وصغيرة وأرقام ورموز");
            }

            if (doctorDto.Correct == false)
                return new BaseResponse<TokenForRegister>(false,"صورة الكارنية غير صحيحة");

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
                return new BaseResponse<TokenForRegister>(false,"فشل إنشاء الحساب");


            await _userManager.AddToRoleAsync(doctor, Roles.Doctor.ToString());

        var response= new TokenForRegister
            {
                Email = doctorDto.Email,

                Token = await _tokenService.GenerateTokenAsync(doctor, _userManager)
            };
            return new BaseResponse<TokenForRegister>(true, "تم انشاء الحساب بنجاح", response);


        }

        

        public async Task<BaseResponse<TokenForRegister>> RegisterForPatientAsync(GetPatientDto patientDto)
        {
    

            if (patientDto.Password != patientDto.ConfirmPassword)
               return new BaseResponse<TokenForRegister>(false,"كلمة المرور وتأكيد كلمة المرور لا يتطابقان");

            if (!new EmailAddressAttribute().IsValid(patientDto.Email))
                return new BaseResponse<TokenForRegister>(false,"تنسيق البريد الإلكتروني غير صالح");

            var existingUser = await _userManager.FindByEmailAsync(patientDto.Email);
            if (existingUser != null)
                return new BaseResponse<TokenForRegister>(false,"يوجد مستخدم لديه هذا البريد الإلكتروني بالفعل");

            var passwordPattern =@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            if (!Regex.IsMatch(patientDto.Password, passwordPattern))
            {
                return new BaseResponse<TokenForRegister>(false,"كلمة المرور يجب أن تحتوي على أحرف كبيرة وصغيرة وأرقام ورموز");
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
               return new BaseResponse<TokenForRegister>(false,"فشل إنشاء الحساب");

            await _userManager.AddToRoleAsync(patient,Roles.Patient.ToString());

            var response= new TokenForRegister
            {
                Email = patientDto.Email,
                Token = await _tokenService.GenerateTokenAsync(patient, _userManager)
            };
            return new BaseResponse<TokenForRegister>(true, "تم انشاء الحساب بنجاح", response);


        }

       

        public async Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            if (resetPassword.NewPassword != resetPassword.ConfirmNewPassword)
                return new BaseResponse<bool>(false, "كلمة المرور وتأكيد كلمة المرور لا يتطابقان");

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null) return new BaseResponse<bool>(false, "لم يتم العثور على بريدك الإلكتروني");

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
             if (!result.Succeeded) return new BaseResponse<bool>(false, "فشلت إعادة تعيين كلمة المرور");

            return new BaseResponse<bool>(true, "تم تحديث كلمة المرور بنجاح");
        }

        public async Task<BaseResponse<bool>> VerifyOTPAsync(VerifyCodeDto verify)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(verify.Email);
                if (user == null)
                    return new BaseResponse<bool>(false, $"Email '{verify.Email}' is not found.");

                var cachedOtp = _memoryCache.Get(verify.Email)?.ToString();
                if (string.IsNullOrEmpty(cachedOtp))
                    return new BaseResponse<bool>(false, "لم يتم العثور على الرمز أو انتهت صلاحيته. يرجى طلب رمز جديد.");

                if (!string.Equals(verify.CodeOTP, cachedOtp, StringComparison.OrdinalIgnoreCase))
                    return new BaseResponse<bool>(false, "الرمز غير صحيح. تأكد من إدخاله بشكل صحيح.");

                // حذف الكود بعد التحقق الناجح
                _memoryCache.Remove(verify.Email);

                return new BaseResponse<bool>(true, "تم التحقق من الرمز بنجاح.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in VerifyOTPAsync: {ex.Message}");
                return new BaseResponse<bool>(false, "حدث خطأ في السيرفر، حاول مرة أخرى لاحقًا.");
            }

        }
    }
}
