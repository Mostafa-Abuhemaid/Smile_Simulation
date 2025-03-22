using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smile_Simulation.Domain.DTOs.Advice;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces;
using Smile_Simulation.Domain.Response;
using Smile_Simulation.Infrastructure.Data;
using Smile_Simulation.Infrastructure.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Application.Services
{
    public class AdviceService : IAdviceService
    {
        private readonly SmileDbContext _context;
        private readonly IConfiguration _configuration;
        public AdviceService(IConfiguration configuration, SmileDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<BaseResponse<Advice>> CreateAdviceAsync(CreateAdviceDTO adviceDto)
        {

           var advice = new Advice
                   {
              
                Title=adviceDto.Title,
                Description=adviceDto.Description,
                CategoryId = adviceDto.CategoryId,
                   
                  };
            if (adviceDto.Image != null)
            {
               var imagePath = Files.UploadFile(adviceDto.Image, "Category");
                advice.Image = $"{_configuration["BaseURL"]}/Advice/{imagePath}";

            }
            advice.VidoeLink = adviceDto.VidoeLink ?? advice.VidoeLink;
            advice.DescriptionOfLink = adviceDto.DescriptionOfLink ?? advice.DescriptionOfLink;


            await _context.Advices.AddAsync(advice);
            await _context.SaveChangesAsync();

            return new BaseResponse<Advice>(true, $"تم اضافة النصيحة  بنجاح", advice);
        }

        public async Task<BaseResponse<bool>> DeleteAdviceAsync(int id)
        {
            var advice = await _context.Advices.FirstOrDefaultAsync(x => x.Id == id);  
            if(advice != null)
            {

                _context.Advices.Remove(advice);
                await _context.SaveChangesAsync();
                  Files.DeleteFile(advice.Image, "Advice");
                return new BaseResponse<bool>(true, "تم حذف النصيحة نجاح");
           
            }
            return new BaseResponse<bool>(false, "النصيحة غير موجودة");
        }

        public async Task<BaseResponse<GetAdviceDTO>> GetAdviceByIdAsync(int id)
        {
            var advice = await _context.Advices.FirstOrDefaultAsync(x=>x.Id == id);
            if( advice != null )
            {
                
                var adviceDTO = new GetAdviceDTO
                {
                    Image = $"{_configuration["BaseURL"]}/Advice/{advice.Image}",
                    Title = advice.Title,
                    Description= advice.Description,
                    VidoeLink= advice.VidoeLink,
                    DescriptionOfLink= advice.DescriptionOfLink,
                    CategoryId= advice.CategoryId,
                    Category= advice.Category.Name
                };
                return new BaseResponse<GetAdviceDTO>(true, "تم استرجاع البيانات بنجاح", adviceDTO);
            }
            return new BaseResponse<GetAdviceDTO>(false, "هذه النصيحة غير موجودة");
        }

        public async Task<BaseResponse<List<GetAdviceDTO>>> GetAllAdviceAsync()
        {
            var advices = await _context.Advices.Select(advice => new GetAdviceDTO
            {

                Image = $"{_configuration["BaseURL"]}/Advice/{advice.Image}",
                Title = advice.Title,
                Description = advice.Description,
                VidoeLink = advice.VidoeLink,
                DescriptionOfLink = advice.DescriptionOfLink,
                CategoryId = advice.CategoryId,
                Category = advice.Category.Name
            }).ToListAsync();
            if( advices != null )
            {
                return new BaseResponse<List<GetAdviceDTO>>(true, "تم استرجاع البيانات بنجاح", advices);
            }
            return new BaseResponse<List<GetAdviceDTO>>(false, "لا يوجد نصائح لعرضها");
        }

        public async Task<BaseResponse<Advice>> UpdateAdviceAsync(int id, CreateAdviceDTO adviceDto)
        {
            var advice = await _context.Advices.FindAsync(id);
            if (advice == null)
            {
                return new BaseResponse<Advice>(false, "النصيحة غير موجودة");
            }

            advice.Title = adviceDto.Title ?? advice.Title;
            advice.Description = adviceDto.Description ?? advice.Description;
            advice.CategoryId = adviceDto.CategoryId;

            if (adviceDto.Image != null)
            {
                if (!string.IsNullOrEmpty(advice.Image))
                {
                    Files.DeleteFile(advice.Image, "Advice"); 
                }

                var imagePath = Files.UploadFile(adviceDto.Image, "Advice");
                advice.Image = $"{_configuration["BaseURL"]}/Advice/{imagePath}";
            }

            
            advice.VidoeLink = adviceDto.VidoeLink ?? advice.VidoeLink;
            advice.DescriptionOfLink = adviceDto.DescriptionOfLink ?? advice.DescriptionOfLink;


            await _context.SaveChangesAsync();

            return new BaseResponse<Advice>(true, "تم تحديث النصيحة بنجاح", advice);
        }

    }
}
