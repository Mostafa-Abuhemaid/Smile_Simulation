using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Smile_Simulation.Domain.DTOs.CategoryDto;
using Smile_Simulation.Domain.Interfaces.Services;

namespace Smile_Simulation.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdviceCategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public AdviceCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAll_Advice_Category")]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            var cat = await _categoryService.GetAllCategoriesAsync();
            return cat.Success ? Ok(cat) : NotFound(cat);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var cat = await _categoryService.GetCategoryByIdAsync(id);
            return cat.Success ? Ok(cat) : NotFound(cat);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var cat = await _categoryService.DeleteCategoryAsync(id);
            return cat.Success ? Ok(cat) : NotFound(cat);
        }
        [HttpPost("Add_Advice_Category")]
        public async Task<IActionResult> AddCategoryAsync([FromForm]CreateCategoryDTO createCategoryDTO)
        {
            var cat = await _categoryService.CreateCategoryAsync(createCategoryDTO);
            return cat.Success ? Ok(cat) : NotFound(cat);
        }
        [HttpPut("Update_Advice_Category/{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id,[FromForm] CreateCategoryDTO createCategoryDTO)
        {
            var cat = await _categoryService.UpdateCategoryAsync(id,createCategoryDTO);
            return cat.Success ? Ok(cat) : NotFound(cat);
        }

    }
}
