using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class VehiclesRefuel
    {
        public int Id { get; set; }

        public int Vehicleid { get; set; }

        public decimal RefuleValue { get; set; }

        public decimal? RefuelCost { get; set; }

        public int? Ododmeter { get; set; }
        public DateTime RefuleDate { get; set; }
        public virtual Vehicle? Vehicle { get; set; } = null!;
    }

}
