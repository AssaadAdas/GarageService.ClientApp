using CommunityToolkit.Mvvm.Input;
using GarageService.ClientApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    [QueryProperty(nameof(VehileId), "vehileid")]
    public class PopupMenuViewModel: BaseViewModel
    {
        public ICommand LoadServicesCommand { get; }
        public PopupMenuViewModel()
        {
            LoadServicesCommand = new Command(async () => await AddServices());
        }
        private int _vehileid;
        public int VehileId
        {
            get => _vehileid;
            set
            {
                _vehileid = value;
            }
        }
        
        private async Task AddServices()
        {
            await Shell.Current.GoToAsync($"{nameof(ServicePage)}?vehileid={VehileId}");
        }
    }
}
