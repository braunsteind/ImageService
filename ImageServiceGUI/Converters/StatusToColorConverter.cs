using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.Converters
{
    class StatusToColorConverter : IValueConverter
    {
        /// <summary>
        /// Converting log status to its relevant color.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.Name != "Brush")
            {
                throw new InvalidOperationException("Must convert to a brush!");
            }

            string status = (string)value;
            switch (status)
            {
                case "INFO":
                    return System.Windows.Media.Brushes.LightGreen;
                case "WARNING":
                    return System.Windows.Media.Brushes.Yellow;
                case "FAIL":
                    return System.Windows.Media.Brushes.Coral;
                default:
                    return Brushes.Transparent;
            }
        }


        /// <summary>
        /// No function implementation.
        /// No need to convert colors to enum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}