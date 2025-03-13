using Smile_Simulation.Domain.DTOs.AccountDto;
using Smile_Simulation.Domain.DTOs.DoctorDto;
using Smile_Simulation.Domain.DTOs.PatientDto;
using Smile_Simulation.Domain.DTOs.TokenDto;

using Smile_Simulation.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Interfaces.Services
{
    public interface IAccountService
    {
       Task<BaseResponse<TokenForRegister>> RegisterForPatientAsync(GetPatientDto patientDTO);
        Task<BaseResponse<TokenForRegister>> RegisterForDoctorAsync(GetDoctorDto doctorDto);
        Task<BaseResponse<TokenDTO>> LoginAsync(LoginDto loginDto);
        Task<BaseResponse<ForgotPasswordDTO>> ForgotPasswordAsync(ForgotDto request);
        Task<BaseResponse<bool>> VerifyOTPAsync(VerifyCodeDto verify);
        Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordDto resetPassword);

    }
}
