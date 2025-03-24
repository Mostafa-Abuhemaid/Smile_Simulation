using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Smile_Simulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.UserDto
{
    public class EditeUserDto
    {
      
        [Required(ErrorMessage = "The FullName is Required")]
        public string FullName { get; set; }
        public int? age { get; set; }
        public IFormFile? Image { get; set; }
        public Gender Gender { get; set; }
        public int? Experience { get; set; } = 0;
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
     
        public string? Address { get; set; } = null;
        public DateOnly? BirthDay { get; set; } = null;
    }
}
