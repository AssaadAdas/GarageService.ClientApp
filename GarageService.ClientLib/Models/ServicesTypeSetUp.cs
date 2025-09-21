using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class ServicesTypeSetUp
    {
        public int Id { get; set; }

        public int ServiceTypesid { get; set; }

        public int ServiceTypesValue { get; set; }

        public int MeassureUnitid { get; set; }

        public int Vehicleid { get; set; }

        public virtual MeassureUnit? MeassureUnit { get; set; } = null!;

        public virtual ServiceType? ServiceTypes { get; set; } = null!;

        public virtual Vehicle? Vehicle { get; set; } = null!;
    }
}
