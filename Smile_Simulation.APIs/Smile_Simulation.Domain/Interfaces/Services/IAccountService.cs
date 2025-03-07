using Smile_Simulation.Domain.DTOs.AccountDto;
using Smile_Simulation.Domain.DTOs.DoctorDto;
using Smile_Simulation.Domain.DTOs.PatientDto;
using Smile_Simulation.Domain.DTOs.TokenDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        public  Task<TokenForRegister> RegisterForPatientAsync(PatientDto patientDTO);
        public Task<TokenForRegister> RegisterForDoctorAsync(DoctorDto doctorDto);
        Task<TokenDTO> LoginAsync(LoginDto loginDto);
        Task<ForgotPasswordDTO> ForgotPasswordAsync(ForgotDto request);
        Task<bool> VerifyOTPAsync(VerifyCodeDto verify);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPassword);

    }
}
