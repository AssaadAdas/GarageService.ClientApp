using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Services.Maps;

namespace GarageService.ClientApp.ViewModels
{
    [QueryProperty(nameof(VehicleId), "vehileid")]
    public  class ServicesTypeSetUpViewModel : BaseViewModel
    {
        public ServiceType ServiceType { get; }
        public int Id => ServiceType.Id;
        public string Description => ServiceType.Description; // or whatever property you use for display

        private int _vehileid;
        public int VehicleId
        {
            get => _vehileid;
            set
            {
                _vehileid = value;
            }
        }

        private decimal _ServiceTypesValue;
        public decimal ServiceTypesValue
        {
            get => _ServiceTypesValue;
            set => SetProperty(ref _ServiceTypesValue, value);
        }
        //MeassureUnitid
        private MeassureUnit _selectedMeassureUnit;
        public MeassureUnit SelectedMeassureUnit
        {
            get => _selectedMeassureUnit;
            set
            {
                SetProperty(ref _selectedMeassureUnit, value);
                MeassureUnitid = value?.Id ?? 0;
                MeassureUnitDesc = value?.MeassureUnitDesc ?? string.Empty;
            }
        }

        private int _MeassureUnitid;
        public int MeassureUnitid
        {
            get => _MeassureUnitid;
            set => SetProperty(ref _MeassureUnitid, value);
        }

        private string _MeassureUnitDesc;
        public string MeassureUnitDesc
        {
            get => _MeassureUnitDesc;
            set => SetProperty(ref _MeassureUnitDesc, value);
        }
        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get => _vehicle;
            set
            {
                if (_vehicle != value)
                {
                    _vehicle = value;
                    OnPropertyChanged(nameof(Vehicle));
                }
            }
        }

        private readonly ApiService _apiService;
        public ICommand LoadVehileCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        public ServicesTypeSetUpViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoadVehileCommand = new Command(async () => await LoadVehicle());
        }
        private async Task LoadVehicle()
        {
            // Get current user ID from your authentication system
            var response = await _apiService.GetVehicleByID(VehicleId);
            if (response.IsSuccess)
            {
                Vehicle = response.Data;
            }
        }
    }
}
