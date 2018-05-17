using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Pente
{
    public class TileStateToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageString = @"Images\PenteEmpty.png";


            if (targetType != typeof(ImageSource))
            {
                throw new Exception("Type of target must be an image");
            }
            

            if((TileState)value == TileState.BLACK)
            {
                imageString = @"Images\PenteBlack.png";
            }
            else if ((TileState)value == TileState.WHITE)
            {
                imageString = @"Images\PenteWhite.png";
            }

            return imageString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
