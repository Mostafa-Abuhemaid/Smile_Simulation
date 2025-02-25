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
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login( LoginDto loginDto)
        {
            
            try
            {
                var result = await _accountService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgotDto request)
        {
            try
            {
                var result = await _accountService.ForgotPasswordAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost("VerifyOTP")]
        public async Task<ActionResult> VerifyOTP([FromBody] VerifyCodeDto verify)
        {

            try
            {
                var result = await _accountService.VerifyOTPAsync(verify);
                return result ? Ok("OTP verified successfully.") : BadRequest("Invalid OTP.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            try
            {
                var result = await _accountService.ResetPasswordAsync(resetPassword);
                return result ? Ok("Password updated successfully.") : BadRequest("Failed to update password.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
