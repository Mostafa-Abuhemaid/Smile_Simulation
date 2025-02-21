using Microsoft.AspNetCore.Identity;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Application.Services
{
    public class TokenService : ITokenService
    {
        public Task<string> GenerateTokenAsync(UserApp user, UserManager<UserApp> userManager)
        {
            throw new NotImplementedException();
        }
    }
}
