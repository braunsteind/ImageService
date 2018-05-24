using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.ViewModel
{
    class BackgroundColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.Name != "Brush")
            {
                throw new InvalidOperationException("Not brush!");
            }

            //check type
            string type = (string)value;
            if (type == "INFO")
            {
                return System.Windows.Media.Brushes.LightGreen;
            }
            if (type == "WARNING")
            {
                return System.Windows.Media.Brushes.Yellow;
            }
            if (type == "FAIL")
            {
                return System.Windows.Media.Brushes.Coral;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}