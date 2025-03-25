using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.UserDto
{
    public class EditUserImageDto
    {
        public IFormFile Image {  get; set; }
    }
}
