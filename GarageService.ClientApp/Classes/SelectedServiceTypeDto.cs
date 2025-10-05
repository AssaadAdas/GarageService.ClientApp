using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientApp.Classes
{
    public class SelectedServiceTypeDto
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public string Notes { get; set; }
        public int CurrId { get; set; }
        public string CurrDesc { get; set; }
    }
}
