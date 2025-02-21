using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Entities
{
    public class Doctor
    {
       
        public string Card {  get; set; }
        public string? qualification { get; set; }
        public string? Specialization { get; set; }
        public string? Experience { get; set; }


    }
}
