using Microsoft.AspNetCore.Http;
using Smile_Simulation.Domain.DTOs.DoctorDto;
using Smile_Simulation.Domain.DTOs.PatientDto;
using Smile_Simulation.Domain.DTOs.UserDto;
using Smile_Simulation.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResponse<SendDoctorDTO>>GetDoctorDetailsAsync(string DoctorId,string role);
        Task<BaseResponse<SendPatientDTO>> GetPatientDetailsAsync(string PatientId,string role);
        Task<BaseResponse<SendDoctorDTO>> EditDoctorDetailsAsync(string userId,EditeUserDto userDto);
        Task<BaseResponse<SendPatientDTO>> EditPatientDetailsAsync(string userId, EditeUserDto userDto);
        Task<BaseResponse<bool>> EditUserImagesAsync(string userId, EditUserImageDto Image);

    }
}
