using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Models
{
    public class UpdateClientPaymentMethodDto
    {
        public int Id { get; set; }
        public int Clientid { get; set; }
        public int PaymentTypeId { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string? CardNumber { get; set; }
        public string? CardHolderName { get; set; }
        public string? Cvv { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastModified { get; set; }
    }
}
