using GarageService.ClientLib.Models;
using System.Windows.Input;

namespace GarageService.ClientApp.Views;

public partial class DashBoardTitle : FlexLayout
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
             nameof(Title),
             typeof(string),
             typeof(DashBoardTitle),
             string.Empty,
             propertyChanged: OnTitleChanged);

    public static readonly BindableProperty SaveCommandProperty = BindableProperty.Create(
        nameof(SaveCommand),
        typeof(ICommand),
        typeof(DashBoardTitle));
    public static readonly BindableProperty IsPremiumProperty =
       BindableProperty.Create(
           nameof(IsPremium),
           typeof(bool),
           typeof(DashBoardTitle),
           false,
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: OnIsPremiumChanged);

    public static readonly BindableProperty PremuimCommandProperty = BindableProperty.Create(
    nameof(PremuimCommand),
    typeof(ICommand),
    typeof(DashBoardTitle));

    public static readonly BindableProperty EditProfileCommandProperty = BindableProperty.Create(
    nameof(PremuimCommand),
    typeof(ICommand),
    typeof(DashBoardTitle));
    //EditProfileCommand
    public static readonly BindableProperty ClientPremiumRegistrationProperty = BindableProperty.Create(
        nameof(ClientPremiumRegistration),
        typeof(ClientPremiumRegistration),
        typeof(DashBoardTitle),
        default(ClientPremiumRegistration),
        propertyChanged: OnClientPremiumRegistrationChanged);

    public static readonly BindableProperty ClientProfileProperty = BindableProperty.Create(
       nameof(ClientProfile),
       typeof(ClientProfile),
       typeof(DashBoardTitle),
       default(ClientProfile),
       propertyChanged: OnClientProfileChanged);

    public DashBoardTitle()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }
    public bool IsPremium
    {
        get => (bool)GetValue(IsPremiumProperty);
        set => SetValue(IsPremiumProperty, value);
    }

    public ICommand PremuimCommand
    {
        get => (ICommand)GetValue(PremuimCommandProperty);
        set => SetValue(PremuimCommandProperty, value);
    }
    public ICommand EditProfileCommand
    {
        get => (ICommand)GetValue(EditProfileCommandProperty);
        set => SetValue(EditProfileCommandProperty, value);
    }


    public ClientPremiumRegistration ClientPremiumRegistration
    {
        get => (ClientPremiumRegistration)GetValue(ClientPremiumRegistrationProperty);
        set => SetValue(ClientPremiumRegistrationProperty, value);
    }

    public ClientProfile ClientProfile
    {
        get => (ClientProfile)GetValue(ClientProfileProperty);
        set => SetValue(ClientProfileProperty, value);
    }


    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (DashBoardTitle)bindable;
        control.TitleLabel.Text = (string)newValue;
    }

    private static void OnIsPremiumChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (DashBoardTitle)bindable;
        // Optional: Add any logic that should run when IsPremium changes
    }
    private static void OnClientPremiumRegistrationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // UI is bound directly to GarageProfile via XAML (see DataTriggers). Keep for future logic.
    }

    private static void OnClientProfileChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // UI is bound directly to GarageProfile via XAML (see DataTriggers). Keep for future logic.
    }
}