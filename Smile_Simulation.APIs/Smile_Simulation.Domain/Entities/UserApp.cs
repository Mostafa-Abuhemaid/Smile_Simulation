﻿using Microsoft.AspNetCore.Identity;
using Smile_Simulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Entities
{
    public class UserApp : IdentityUser
    {
        public string FullName { get; set; }
        public string? Image { get; set; }
        public Gender gender { get; set; }
    }
}
