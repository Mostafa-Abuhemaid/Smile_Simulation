using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Male = 0,
        Female = 1,
    }
}
