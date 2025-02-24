using Microsoft.AspNetCore.Http;
using Smile_Simulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.DoctorDto
{
    public class DoctorDto
    {

        [Required(ErrorMessage = "The FullName is Required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "The Email is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Password is Required")]
  
        public string Password { get; set; }
        [Required(ErrorMessage = "The ConfirmPassword is Required")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "The Image is Required")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "The Card is Required")]
        public IFormFile Card { get; set; }

        [Required(ErrorMessage = "The Gender is Required")]
        public Gender Gender { get; set; }

        public int? Experience { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public bool Correct { get; set; }=false;


    }
}
