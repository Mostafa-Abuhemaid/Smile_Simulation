using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.AccountDto
{
    public class VerifyCodeDto
    {
        public string Email { get; set; }
        public string CodeOTP { get; set; }
    }
}
