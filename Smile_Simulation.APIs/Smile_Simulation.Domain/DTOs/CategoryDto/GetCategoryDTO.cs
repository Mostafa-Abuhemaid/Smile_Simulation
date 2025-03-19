using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.CategoryDto
{
    public class GetCategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name is required")]
        public string Name { get; set; }

        public string Image { get; set; } 
    }
}
