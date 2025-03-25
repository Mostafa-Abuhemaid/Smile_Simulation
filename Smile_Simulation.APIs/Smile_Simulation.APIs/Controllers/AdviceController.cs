using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smile_Simulation.Domain.DTOs.Advice;
using Smile_Simulation.Domain.Interfaces;

namespace Smile_Simulation.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdviceController : ControllerBase
    {
        private readonly IAdviceService _adviceService;
        public AdviceController(IAdviceService adviceService)
        {
            _adviceService = adviceService;
        }


        [HttpGet("GetAllAdvices")]
        public async Task<IActionResult> GetAllAdviceAsync()
        {
            var cat = await _adviceService.GetAllAdviceAsync();
            return cat.Success ? Ok(cat) : NotFound(cat);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdviceByIdAsync(int id)
        {
            var cat = await _adviceService.GetAdviceByIdAsync(id);
            return cat.Success ? Ok(cat) : NotFound(cat);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdviceAsync(int id)
        {
            var cat = await _adviceService.DeleteAdviceAsync(id);
            return cat.Success ? Ok(cat) : NotFound(cat);
        }
        [HttpPut("UpdateAdvice")]
        public async Task<IActionResult> UpdateAdviceAsync([FromQuery]int id ,[FromForm] CreateAdviceDTO createAdviceDTO)
        {
            var cat = await _adviceService.UpdateAdviceAsync(id,createAdviceDTO);
            return cat.Success ? Ok(cat) : NotFound(cat);
        }
        [HttpPost()]
        public async Task<IActionResult> CreateAdviceAsync([FromForm] CreateAdviceDTO createAdviceDTO)
        {
            var cat = await _adviceService.CreateAdviceAsync(createAdviceDTO);
            return cat.Success ? Ok(cat) : NotFound(cat);
        }
    }
}
