using GarageService.ClientLib.Models;
using Microsoft.Maui.Controls;
using System.IO;

namespace GarageService.ClientApp.ViewModels
{
    public class CountryDisplayItem : BaseViewModel
    {
        public Country Country { get; }

        public int Id => Country?.Id ?? 0;
        public string CountryName => Country?.CountryName ?? string.Empty;

        private ImageSource _flagImage;
        public ImageSource FlagImage
        {
            get => _flagImage;
            set => SetProperty(ref _flagImage, value);
        }

        public CountryDisplayItem(Country country)
        {
            Country = country;
            FlagImage = CreateImageSource(country?.CountryFlag);
        }

        private ImageSource CreateImageSource(byte[]? bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }
    }
}
