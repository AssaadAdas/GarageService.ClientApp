using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class ServiceType
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public bool? IsSelected { get; set; }

        public virtual ICollection<ServicesTypeSetUp> ServicesTypeSetUps { get; set; } = new List<ServicesTypeSetUp>();

        public virtual ICollection<VehiclesServiceType> VehiclesServiceTypes { get; set; } = new List<VehiclesServiceType>();
    }

}
