using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smile_Simulation.Domain.DTOs.AccountDto;
using Smile_Simulation.Domain.DTOs.DoctorDto;
using Smile_Simulation.Domain.DTOs.PatientDto;
using Smile_Simulation.Domain.DTOs.TokenDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Enums;
using Smile_Simulation.Domain.Interfaces.Services;
using Smile_Simulation.Domain.Response;


namespace Smile_Simulation.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
     
        private readonly IAccountService _accountService;
        public AccountController( IAccountService accountService)
        {
       
            _accountService = accountService;
        }
        [HttpPost("Register/Patient")]
        public async Task<IActionResult> RegisterForPatient([FromForm]PatientDto patientDto)
        {
            try
            {
                var result = await _accountService.RegisterForPatientAsync(patientDto);
                var response = new BaseResponse<TokenForRegister>(true, "تم انشاء الحساب بنجاح", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                
                var errorResponse = new BaseResponse<object>(false, ex.Message);
                return BadRequest(errorResponse);
            }

        }
        [HttpPost("Register/Doctor")]
        public async Task<IActionResult> RegisterForDoctor([FromForm] DoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _accountService.RegisterForDoctorAsync(doctorDto);
                var response = new BaseResponse<TokenForRegister>(true, "تم انشاء الحساب بنجاح", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new BaseResponse<object>(false, ex.Message);
                return BadRequest(errorResponse);
            }

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login( LoginDto loginDto)
        {
            
            try
            {
                var result = await _accountService.LoginAsync(loginDto);
                var response = new BaseResponse<TokenDTO>(true, "تم تسجيل الدخول بنجاح", result);
                return Ok(response);

             
            }
            catch (Exception ex)
            {
                  var errorResponse = new BaseResponse<object>(false, ex.Message);
                  return BadRequest(errorResponse);
            }

        }
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgotDto request)
        {
            try
            {
                var result = await _accountService.ForgotPasswordAsync(request);
                var response = new BaseResponse<ForgotPasswordDTO>(true, "تحقق من بريدك الاكتروني", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new BaseResponse<object>(false, ex.Message);
                return BadRequest(errorResponse);
            }

        }

        [HttpPost("VerifyOTP")]
        public async Task<ActionResult> VerifyOTP([FromBody] VerifyCodeDto verify)
        {

            try
            {
                var result = await _accountService.VerifyOTPAsync(verify);
                var response = new BaseResponse<ForgotPasswordDTO>(true, "تم التحقق من الرمز بنجاح");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new BaseResponse<object>(false, ex.Message);
                return BadRequest(errorResponse);
            }
        }
        [HttpPut("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            try
            {
                var result = await _accountService.ResetPasswordAsync(resetPassword);
                var response = new BaseResponse<ForgotPasswordDTO>(true, "تم تحديث كلمة المرور بنجاح");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new BaseResponse<object>(false, ex.Message);
                return BadRequest(errorResponse);
            }
        }
    }
}
