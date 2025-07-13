using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace GarageService.ClientApp.Views
{
    public partial class TitleView : FlexLayout
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(TitleView),
            string.Empty,
            propertyChanged: OnTitleChanged);

        public static readonly BindableProperty SaveCommandProperty = BindableProperty.Create(
            nameof(SaveCommand),
            typeof(ICommand),
            typeof(TitleView));

        public static readonly BindableProperty BackCommandProperty = BindableProperty.Create(
            nameof(BackCommand),
            typeof(ICommand),
            typeof(TitleView));
        public TitleView()
        {
            InitializeComponent();

            // Set default back command if none provided
            if (BackCommand == null)
            {
                BackCommand = new Command(async () =>
                {
                    if (Shell.Current?.Navigation != null && Shell.Current.Navigation.NavigationStack.Count > 1)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                });
            }
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

        public ICommand BackCommand
        {
            get => (ICommand)GetValue(BackCommandProperty);
            set => SetValue(BackCommandProperty, value);
        }

        private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TitleView)bindable;
            control.TitleLabel.Text = (string)newValue;
        }
    }
}