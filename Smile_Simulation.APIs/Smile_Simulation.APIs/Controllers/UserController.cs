using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smile_Simulation.Application.Services;
using Smile_Simulation.Domain.DTOs.UserDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Enums;
using Smile_Simulation.Domain.Interfaces.Services;
using Smile_Simulation.Domain.Response;
using System.Security.Claims;

namespace Smile_Simulation.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly UserManager<UserApp> _userManager;
        public UserController(IUserService userService, UserManager<UserApp> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetils()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }
            if (role == Roles.Doctor.ToString())
            {
                var result = await _userService.GetDoctorDetailsAsync(userId, role);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            else
            {
                var result = await _userService.GetPatientDetailsAsync(userId, role);
                return result.Success ? Ok(result) : BadRequest(result);
            }
        }
        [Authorize]
        [HttpPut("EditUserDetails")]
        public async Task<IActionResult> EditUserDetils([FromForm]EditeUserDto userDto)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }
           if (role == Roles.Doctor.ToString())
           {
               var result = await _userService.EditDoctorDetailsAsync(userId,userDto);
               return result.Success ? Ok(result) : BadRequest(result);
           }
           else
           {
               var result = await _userService.EditPatientDetailsAsync(userId, userDto);
               return result.Success ? Ok(result) : BadRequest(result);
           }
        }
        [Authorize]
        [HttpPut("EditUserImage")]
        public async Task<IActionResult> EditUserImage([FromForm] EditUserImageDto Image)
        {
            if (Image == null)
            {
                return BadRequest("Image file is required.");
            }
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }            
                var result = await _userService.EditUserImagesAsync(userId, Image);
                return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
