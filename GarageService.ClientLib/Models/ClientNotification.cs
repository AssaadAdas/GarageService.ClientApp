using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class ClientNotification
    {
        public int Id { get; set; }

        public int Clientid { get; set; }

        public string? Notes { get; set; }
        public bool IsRead { get; set; } = false;

        public virtual ClientProfile Client { get; set; } = null!;
    }

}
