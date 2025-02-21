using Microsoft.AspNetCore.Identity;
using Smile_Simulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(UserApp user, UserManager<UserApp> userManager);
    }
}
