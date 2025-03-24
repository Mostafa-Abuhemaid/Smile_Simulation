using Microsoft.AspNetCore.Http;
using Smile_Simulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.Advice
{
    public class CreateAdviceDTO
    {
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "The Title is requred")]

        public string Title { get; set; }

        [Required(ErrorMessage = "The Description is requred")]
        public string Description { get; set; }

        public string? Link { get; set; }

        public string? DescriptionOfLink { get; set; }
        [Required(ErrorMessage = "The Link is requred")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")]
        public int CategoryId { get; set; }

    }
}
