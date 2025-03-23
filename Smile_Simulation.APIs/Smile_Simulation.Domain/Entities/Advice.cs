using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Entities
{
    public class Advice : BaseEntity<int>
    {

        public string? Image { get; set; }

        [Required(ErrorMessage = "The Title is requred")]
       
        public string Title { get; set; }

        [Required(ErrorMessage = "The Description is requred")]
        public string Description { get; set; }

        public string? Link { get; set; }

        public string? DescriptionOfLink { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

    }
}
