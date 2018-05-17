using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
                    imageString = @"..\..\Images\PenteEmpty.png";
                    break;
                case TileState.WHITE:
                    imageString = @"..\..\Images\PenteWhite.png";
                    break;
                case TileState.BLACK:
                    imageString = @"..\..\Images\PenteBlack.png";
                    break;
            }

            return new BitmapImage(new Uri(Path.GetFullPath(imageString)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
