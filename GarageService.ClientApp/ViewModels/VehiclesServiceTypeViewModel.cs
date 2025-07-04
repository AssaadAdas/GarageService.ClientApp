using GarageService.ClientLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientApp.ViewModels
{
    public class VehiclesServiceTypeViewModel:BaseViewModel
    {
        private VehiclesServiceType _serviceType;

        public event PropertyChangedEventHandler PropertyChanged;

        public VehiclesServiceTypeViewModel(VehiclesServiceType serviceType)
        {
            _serviceType = serviceType;
        }

        public int VehicleServiceId
        {
            get => _serviceType.VehicleServiceId;
            set { _serviceType.VehicleServiceId = value; OnPropertyChanged(); }
        }

        public int ServiceTypeId
        {
            get => _serviceType.ServiceTypeId;
            set { _serviceType.ServiceTypeId = value; OnPropertyChanged(); }
        }

        public decimal Cost
        {
            get => _serviceType.Cost;
            set { _serviceType.Cost = value; OnPropertyChanged(); }
        }

        public int CurrencyId
        {
            get => _serviceType.CurrId;
            set { _serviceType.CurrId = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => _serviceType.Notes ?? string.Empty;
            set { _serviceType.Notes = value; OnPropertyChanged(); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
