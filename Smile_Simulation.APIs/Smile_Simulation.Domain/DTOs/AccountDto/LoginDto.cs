using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.AccountDto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "The Email is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Password is Required")]
        public string Password { get; set; }

    }
}
