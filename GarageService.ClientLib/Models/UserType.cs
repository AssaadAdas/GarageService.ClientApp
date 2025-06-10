using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{

    public partial class UserType
    {
        public int Id { get; set; }

        public string? UserTypeDesc { get; set; }

        public virtual ICollection<PremiumOffer>? PremiumOffers { get; set; } = new List<PremiumOffer>();

        public virtual ICollection<User>? Users { get; set; } = new List<User>();
    }

}
