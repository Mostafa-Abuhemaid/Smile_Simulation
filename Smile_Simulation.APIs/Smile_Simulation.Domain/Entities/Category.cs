using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Entities
{
    public class Category :BaseEntity<int>
    {
        [Required(ErrorMessage = "The Name is requred")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Image is requred")]
        public string Image { get; set; }
        public ICollection<Advice> advices { get; set; } = new HashSet<Advice>();
    }
}
