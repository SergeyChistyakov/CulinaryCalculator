using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace CulinaryCalculator.Infrastructure
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var byteArray = value as byte[];
            if (byteArray == null) return null;
            var imageSource = ImageSource.FromStream(()=> new MemoryStream(byteArray));
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
