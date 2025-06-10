using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class Manufacturer
    {
        public int Id { get; set; }

        public string ManufacturerDesc { get; set; } = null!;

        public byte[]? ManufacturerLogo { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }

}
