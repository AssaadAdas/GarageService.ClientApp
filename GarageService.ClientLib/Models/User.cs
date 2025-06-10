using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int UserTypeid { get; set; }

        public virtual ICollection<ClientProfile>? ClientProfiles { get; set; } = new List<ClientProfile>();

        public virtual ICollection<GarageProfile>? GarageProfiles { get; set; } = new List<GarageProfile>();

        public virtual UserType? UserType { get; set; } = null!;
    }
}
