using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{
    public class VehicleHistoryResponse
    {
        public List<VehicleAppointment> Appointments { get; set; }
        public List<VehicleCheck> Checks { get; set; }
        public List<VehiclesRefuel> Refuels { get; set; }
        public List<VehiclesService> Services { get; set; }
    }
}
