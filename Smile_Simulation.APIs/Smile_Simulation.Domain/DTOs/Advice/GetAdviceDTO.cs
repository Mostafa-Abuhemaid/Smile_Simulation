using Smile_Simulation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.Advice
{
    public class GetAdviceDTO
    {
        public int Id {  get; set; }
        public string? Image { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string? Link { get; set; }

        public string? DescriptionOfLink { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }
    }
}
