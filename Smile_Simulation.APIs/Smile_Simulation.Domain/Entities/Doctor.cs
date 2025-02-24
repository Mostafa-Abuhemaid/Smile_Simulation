using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Entities
{
    public class Doctor :UserApp
    {
       
        public string Card {  get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public int? Experience { get; set; }


    }
}
