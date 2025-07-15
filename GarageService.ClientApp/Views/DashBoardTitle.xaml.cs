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

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (DashBoardTitle)bindable;
        control.TitleLabel.Text = (string)newValue;
    }
}