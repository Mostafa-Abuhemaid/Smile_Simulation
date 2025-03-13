using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smile_Simulation.Domain.DTOs.AccountDto;
using Smile_Simulation.Domain.DTOs.TokenDto;
using Smile_Simulation.Domain.DTOs.DoctorDto;
using Smile_Simulation.Domain.DTOs.PatientDto;
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
        public async Task<ActionResult<BaseResponse<TokenForRegister>>> RegisterForPatient([FromForm]GetPatientDto patientDto)
        {
          
                var result = await _accountService.RegisterForPatientAsync(patientDto);
                return result.Success ? Ok(result) : BadRequest(result);

          }
        [HttpPost("Register/Doctor")]
        public async Task<ActionResult<BaseResponse<TokenForRegister>>> RegisterForDoctor([FromForm] GetDoctorDto doctorDto)
        {
          
                var result = await _accountService.RegisterForDoctorAsync(doctorDto);
                return result.Success ? Ok(result) : BadRequest(result);

         }
        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse<TokenDTO>>> Login(LoginDto loginDto)
        {
            var result = await _accountService.LoginAsync(loginDto);
            return result.Success ? Ok(result) : BadRequest(result);

        }
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<BaseResponse<ForgotPasswordDTO>>> ForgetPassword([FromBody] ForgotDto request)
        {
          
             var result = await _accountService.ForgotPasswordAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("VerifyOTP")]
        public async Task<ActionResult<BaseResponse<bool>>> VerifyOTP([FromBody] VerifyCodeDto verify)
        {
  
                var result = await _accountService.VerifyOTPAsync(verify);
                return result.Success ? Ok(result) : BadRequest(result);
            }
        [HttpPut("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {   
                var result = await _accountService.ResetPasswordAsync(resetPassword);
                return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
