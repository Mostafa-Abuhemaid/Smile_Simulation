using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smile_Simulation.Domain.DTOs.Advice;
using Smile_Simulation.Domain.DTOs.CategoryDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces.Services;
using Smile_Simulation.Domain.Response;
using Smile_Simulation.Infrastructure.Data;
using Smile_Simulation.Infrastructure.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smile_Simulation.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly SmileDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public CategoryService(SmileDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<BaseResponse<bool>> CreateCategoryAsync(CreateCategoryDTO categoryDto)
        {
            try
            {
                if (categoryDto.Image == null)
                {
                    return new BaseResponse<bool>(false, "يجب رفع صورة للقسم.");
                }

                var imagePath = Files.UploadFile(categoryDto.Image, "Category");

                var category = new Category
                {
                    Name = categoryDto.Name,
                    Image = imagePath
                };

                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();

                return new BaseResponse<bool>(true, $"تم اضافة قسم {category.Name} بنجاح", true);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, $"حدث خطأ أثناء إضافة القسم: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _dbContext.Categories.FindAsync(id);
                if (category == null)
                {
                    return new BaseResponse<bool>(false, "القسم غير موجود.");
                }
                Files.DeleteFile(category.Image, "Catecory");
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
                
                return new BaseResponse<bool>(true, "تم حذف القسم بنجاح", true);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, $"حدث خطأ أثناء حذف القسم: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<GetCategoryDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                string baseUrl = _configuration["BaseURL"] ?? "http://smilesimulation.runasp.net";

                var categories = await _dbContext.Categories
                    .Select(c => new GetCategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Image = c.Image
                    })
                    .ToListAsync();

                for (int i = 0; i < categories.Count; i++)
                {
                    categories[i].Image = $"{baseUrl}/category/{categories[i].Image}";
                }

                if (categories.Any())
                {
                    return new BaseResponse<List<GetCategoryDTO>>(true, "تم استرجاع جميع الأقسام بنجاح", categories);
                }

                return new BaseResponse<List<GetCategoryDTO>>(false, "لا توجد أقسام متاحة");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching categories: {ex}");
                return new BaseResponse<List<GetCategoryDTO>>(false, "حدث خطأ أثناء جلب البيانات");
            }
        }

        public async Task<BaseResponse<List<GetAdviceDTO>>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var advices = await _dbContext.Advices.Where(A => A.CategoryId == id).Include(C => C.Category).ToListAsync();
                if (advices == null)
                {
                    return new BaseResponse<List<GetAdviceDTO>>(false, "لا يوجد نصائح في هذا القسم");
                }
                var adviceDtos = advices.Select(a => new GetAdviceDTO
                {
                    Id = a.Id,
                    Image = a.Image,
                    Title = a.Title,
                    Description = a.Description,
                    Link = a.Link,
                    DescriptionOfLink = a.DescriptionOfLink,
                    CategoryId = a.CategoryId,
                    Category = a.Category.Name // Assuming Category has a "Name" property
                }).ToList();

                return new BaseResponse<List<GetAdviceDTO>>(true, "تم استرجاع نصائح القسم بنجاح", adviceDtos);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching category by ID: {ex}");
                return new BaseResponse<List < GetAdviceDTO >> (false, "حدث خطأ أثناء جلب البيانات");
            }
        }

        public async Task<BaseResponse<bool>> UpdateCategoryAsync(int id,CreateCategoryDTO categoryDto)
        {
            try
            {
                var category = await _dbContext.Categories.FindAsync(id);
                if (category == null)
                {
                    return new BaseResponse<bool>(false, "القسم غير موجود.");
                }

                if (categoryDto.Image != null)
                {
                    Files.DeleteFile(category.Image, "Catecory");
                    var imagePath = Files.UploadFile(categoryDto.Image, "Category");
                    category.Image = imagePath;
                }

                category.Name = categoryDto.Name;
                _dbContext.Categories.Update(category);
                await _dbContext.SaveChangesAsync();

                return new BaseResponse<bool>(true, "تم تحديث القسم بنجاح", true);
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, $"حدث خطأ أثناء تحديث القسم: {ex.Message}");
            }
         
        }
    }
}