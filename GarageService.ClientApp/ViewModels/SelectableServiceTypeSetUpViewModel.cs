using GarageService.ClientLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageService.ClientApp.ViewModels
{
    public  class SelectableServiceTypeSetUpViewModel : BaseViewModel
    {
        public ServiceType ServiceType { get; }
        public int Id => ServiceType.Id;
        public string Description => ServiceType.Description; // or whatever property you use for display
        //ServiceTypesValue
        private int _ServiceTypesValue;
        public int ServiceTypesValue
        {
            get => _ServiceTypesValue;
            set => SetProperty(ref _ServiceTypesValue, value);
        }

        //MeassureUnitid
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

        private MeassureUnit _MeassureUnit;
        public MeassureUnit SelectedMeassureUnit
        {
            get => _MeassureUnit;
            set
            {
                SetProperty(ref _MeassureUnit, value);
                MeassureUnitid = value?.Id ?? 0;
                MeassureUnitDesc = value?.MeassureUnitDesc ?? string.Empty;
            }
        }
        //Vehicleid
        public SelectableServiceTypeSetUpViewModel(ServiceType serviceType)
        {
            ServiceType = serviceType;
        }
    }
}
