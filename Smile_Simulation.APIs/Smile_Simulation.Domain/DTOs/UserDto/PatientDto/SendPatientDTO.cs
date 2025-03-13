using Microsoft.AspNetCore.Http;
using Smile_Simulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.PatientDto
{
    public class SendPatientDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }
    
        public string Image { get; set; }
       
        public Gender gender { get; set; }
        public string role { get; set; }
        public string? Address { get; set; } = null;
        public DateOnly? BirthDay { get; set; } = null;
    }
}
