using Smile_Simulation.Domain.DTOs.DoctorDto;
using Smile_Simulation.Domain.DTOs.PatientDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<SendDoctorDTO> GetDoctorDetailsAsync(string DoctorId);
        Task<SendPatientDTO> GetPatientDetailsAsync(string PatientId);
    }
}
