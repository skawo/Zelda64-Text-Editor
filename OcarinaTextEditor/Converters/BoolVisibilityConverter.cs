using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OcarinaTextEditor.Converters
{
    /// <summary>
    /// Null-to-Bool converter. If an object is null, Convert returns false.
    /// </summary>
    public class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType,
          object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
