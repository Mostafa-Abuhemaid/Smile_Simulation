using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.AccountDto
{
    public class ForgotDto
    {
        [Required(ErrorMessage = "EmailAddress is Required")]
        [EmailAddress]
        public string Email { get; set; }
    }

}


