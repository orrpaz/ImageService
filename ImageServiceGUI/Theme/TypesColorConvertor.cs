using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.Theme
{
    class TypesColorConvertor : IValueConverter
    {
        /// <summary>
        /// Convert value to appropriate color
        /// </summary>
        /// <param name="value">Name of log status</param>
        /// <param name="targetType">type of brush</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("Must convert to a brush!");
            }
            object brush;
            switch ((string)value)
            {
                case "INFO":
                    brush = Brushes.LawnGreen;
                    break;
                case "WARNING":
                    brush = Brushes.Yellow;
                    break;
                case "FAIL":
                    brush = Brushes.Tomato;
                    break;
                default:
                    brush = Brushes.Transparent;
                    break;
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
