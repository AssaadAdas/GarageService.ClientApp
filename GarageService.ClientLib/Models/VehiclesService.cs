using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class VehiclesService
    {
        public int Id { get; set; }

        public DateTime ServiceDate { get; set; }

        public int Odometer { get; set; }

        public string ServiceLocation { get; set; } = null!;

        public string? Notes { get; set; }

        public int Vehicleid { get; set; }

        public int Garageid { get; set; }

        public virtual GarageProfile? Garage { get; set; } = null!;

        public virtual Vehicle? Vehicle { get; set; } = null!;

        public virtual ICollection<VehiclesServiceType>? VehiclesServiceTypes { get; set; } = new List<VehiclesServiceType>();
    }

}
