using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class Specialization
    {
        public int Id { get; set; }

        public string SpecializationDesc { get; set; } = null!;

        public virtual ICollection<GarageProfile> GarageProfiles { get; set; } = new List<GarageProfile>();
    }
}
