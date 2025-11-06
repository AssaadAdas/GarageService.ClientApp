using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Maui.Controls;

namespace GarageService.ClientApp.Converters
{
    public class ByteToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return null;

                // already an ImageSource
                if (value is ImageSource img) return img;

                // byte[]
                if (value is byte[] bytes && bytes.Length > 0)
                    return ImageSource.FromStream(() => new MemoryStream(bytes));

                // base64 string (optionally data URL)
                if (value is string s && !string.IsNullOrWhiteSpace(s))
                {
                    var idx = s.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
                    if (idx >= 0) s = s.Substring(idx + 7);
                    try
                    {
                        var b = System.Convert.FromBase64String(s);
                        return ImageSource.FromStream(() => new MemoryStream(b));
                    }
                    catch (FormatException fe)
                    {
                        
                        return null;
                    }
                }

                
                return null;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
