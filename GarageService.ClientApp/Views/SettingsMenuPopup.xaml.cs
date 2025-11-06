using CommunityToolkit.Maui.Views;
using GarageService.ClientApp.ViewModels;
using System.Windows.Input;

namespace GarageService.ClientApp.Views;

public partial class SettingsMenuPopup : Popup
{
    public ICommand GoPaymentMethodsCommand { get; }
    public ICommand ChangePasswordMethodsCommand { get; }
    private readonly Popup _popup;
    public int _ClientID;
    public SettingsMenuPopup()
	{
		InitializeComponent();
        
        GoPaymentMethodsCommand = new Command(async () => await GoPaymentMethods());
        this.BindingContext = this; // Important: Set BindingContext to self
    }
   
    private async Task GoPaymentMethods()
    {
        await Shell.Current.GoToAsync($"{nameof(PaymentMethodsPage)}");
    }
}