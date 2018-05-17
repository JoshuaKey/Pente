using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pente
{
    public class Piece : IValueConverter
    {
        public TileState TileState { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TileState state = (TileState)value;

            string imageString = "";

            switch (state)
            {
                case TileState.EMPTY:
                    imageString = @"C:\Users\Colin Misbach\Documents\School Asignments\Quarter 7\Principles of Programming\XP\Pente\Pente\Images\PenteEmpty.png";
                    break;
                case TileState.WHITE:
                    imageString = @"C:\Users\Colin Misbach\Documents\School Asignments\Quarter 7\Principles of Programming\XP\Pente\Pente\Images\PenteWhite.png";
                    break;
                case TileState.BLACK:
                    imageString = @"C:\Users\Colin Misbach\Documents\School Asignments\Quarter 7\Principles of Programming\XP\Pente\Pente\Images\PenteBlack.png";
                    break;
            }

            Image i = new Image();
            i.Source = new BitmapImage(new Uri(imageString));

            return i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
