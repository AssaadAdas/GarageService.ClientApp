using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class VehiclesServiceType
    {
        public int VehicleServiceId { get; set; }

        public int ServiceTypeId { get; set; }

        public decimal Cost { get; set; }

        public int CurrId { get; set; }

        public string? Notes { get; set; }

        public virtual Currency? Curr { get; set; } = null!;

        public virtual ServiceType? ServiceType { get; set; } = null!;

        public virtual VehiclesService? VehicleService { get; set; } = null!;
    }

}
