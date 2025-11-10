using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{
    public class PasswordChangeRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
