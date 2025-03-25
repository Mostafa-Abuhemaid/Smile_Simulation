using Microsoft.AspNetCore.Components.Web;
using Smile_Simulation.Domain.DTOs.Advice;
using Smile_Simulation.Domain.DTOs.CategoryDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<BaseResponse<List<GetCategoryDTO>>> GetAllCategoriesAsync();
        Task<BaseResponse<List<GetAdviceDTO>>> GetCategoryByIdAsync(int id);
        Task<BaseResponse<bool>> CreateCategoryAsync(CreateCategoryDTO categoryDto);
        Task<BaseResponse<bool>> UpdateCategoryAsync(int id,CreateCategoryDTO categoryDto);
        Task<BaseResponse<bool>> DeleteCategoryAsync(int id);
    }
}
