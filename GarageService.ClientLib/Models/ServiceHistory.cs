using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{
    public class ServiceHistory
    {
        public string Description { get; set; }
        public int Odometer { get; set; }
        public DateTime ServiceDate { get; set; }
        public string? Notes { get; set; }
    }
}
