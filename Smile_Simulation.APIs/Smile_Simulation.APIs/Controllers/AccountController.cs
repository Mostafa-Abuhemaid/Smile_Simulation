using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly UserManager<UserApp> _userManager;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;
        public AccountController(UserManager<UserApp> userManager, SignInManager<UserApp> signInManager, IConfiguration configuration, ITokenService tokenService, IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _accountService = accountService;
        }
        [HttpPost("Register/Patient")]
        public async Task<IActionResult> Register([FromForm]PatientDto patientDto)
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
    }
}
