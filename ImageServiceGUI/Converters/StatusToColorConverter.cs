using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.Converters
{
    class StatusToColorConverter : IValueConverter
    {
        //Convert color
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.Name != "Brush")
            {
                throw new InvalidOperationException("Must convert to a brush!");
            }
            //Check type
            if ((string)value == "INFO")
            {
                return System.Windows.Media.Brushes.LimeGreen;
            }
            if ((string)value == "WARNING")
            {
                return System.Windows.Media.Brushes.Yellow;
            }
            if ((string)value == "FAIL")
            {
                return System.Windows.Media.Brushes.OrangeRed;
            }
            return Brushes.Transparent;
        }
        
        /// <summary>
        /// Ignore
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}