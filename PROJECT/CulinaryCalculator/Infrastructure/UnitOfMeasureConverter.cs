using CulinaryCalculator.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CulinaryCalculator.Infrastructure
{
    public class UnitOfMeasureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var unitOfMeasure = (UnitOfMeasure)value;

            switch (unitOfMeasure)
            {
                case UnitOfMeasure.Gram: return "гр";
                case UnitOfMeasure.Item: return "шт";
                case UnitOfMeasure.Milliliter: return "мл";
                case UnitOfMeasure.TableSpoon: return "c.л.";
                case UnitOfMeasure.TeaSpoon: return "ч.л.";
            }

            throw new ArgumentOutOfRangeException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var abbreviation = (string)value;

            switch (abbreviation)
            {
                case "гр": return UnitOfMeasure.Gram;
                case "шт": return UnitOfMeasure.Item;
                case "мл": return UnitOfMeasure.Milliliter;
                case "c.л.": return UnitOfMeasure.TableSpoon;
                case "ч.л.": return UnitOfMeasure.TeaSpoon;
            }

            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}
