using Smile_Simulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.TokenDto
{
    public class TokenForRegister
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
