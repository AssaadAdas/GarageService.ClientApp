using CommunityToolkit.Maui.Views;
using GarageService.ClientApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    public class SettingsMenuViewModel: BaseViewModel
    {
        private readonly Popup _popup;
        public ICommand GoPaymentMethodsCommand { get; }
        public ICommand ChangePasswordMethodsCommand { get; }

        public SettingsMenuViewModel(Popup popup)
        {
            _popup = popup;
            GoPaymentMethodsCommand = new Command(async () => await GoPaymentMethods());
        }
        private async Task GoPaymentMethods()
        {
            await Shell.Current.GoToAsync($"{nameof(PaymentMethodsPage)}");
        }
    }
}
