using Smile_Simulation.Domain.DTOs.Advice;
using Smile_Simulation.Domain.DTOs.CategoryDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Interfaces
{
    public interface IAdviceService
    {
        Task<BaseResponse<List<GetAdviceDTO>>> GetAllAdviceAsync();
        Task<BaseResponse<GetAdviceDTO>> GetAdviceByIdAsync(int id);
        Task<BaseResponse<Advice>> CreateAdviceAsync(CreateAdviceDTO adviceDto);
        Task<BaseResponse<Advice>> UpdateAdviceAsync(int id, CreateAdviceDTO adviceDto);
        Task<BaseResponse<bool>> DeleteAdviceAsync(int id);
    }
}
