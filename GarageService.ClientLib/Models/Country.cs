using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class Country
    {
        public int Id { get; set; }

        public string CountryName { get; set; }

        public string? PhoneExt { get; set; }

        public byte[]? CountryFlag { get; set; }

        public virtual ICollection<ClientProfile> ClientProfiles { get; set; } = new List<ClientProfile>();

        public virtual ICollection<GarageProfile> GarageProfiles { get; set; } = new List<GarageProfile>();
    }

}
