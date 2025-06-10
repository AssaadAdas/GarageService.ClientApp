using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class ClientReminder
    {
        public int Id { get; set; }

        public int Clientid { get; set; }

        public DateTime? ReminderDate { get; set; }

        public string? Notes { get; set; }

        public virtual ClientProfile IdNavigation { get; set; } = null!;
    }

}
