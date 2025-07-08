using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    public class SelectableServiceTypeViewModel : BaseViewModel
    {
        public ServiceType ServiceType { get; }
        public int Id => ServiceType.Id;
        public string Description => ServiceType.Description; // or whatever property you use for display

        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        private Currency _selectedCurrency;
        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                SetProperty(ref _selectedCurrency, value);
                CurrId = value?.Id ?? 0;
            }
        }

        private int _currId;
        public int CurrId
        {
            get => _currId;
            set => SetProperty(ref _currId, value);
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public bool IsSelected { get; set; }
        public SelectableServiceTypeViewModel(ServiceType serviceType)
        {
            ServiceType = serviceType;
        }
    }
}
