﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	[Serializable]
    public class Piece : IValueConverter, INotifyPropertyChanged
    {
        private TileState state = TileState.EMPTY;
        public TileState TileState
        {
            get { return state; }
            set
            {
                state = value;
                NotifyStateChanged("TileState");
            }
        }
        public static string EmptyPath { get { return @"..\..\..\Pente\Images\PenteEmpty.png"; } }
        public static string BlackPath { get { return @"..\..\..\Pente\Images\PenteBlackCross.png"; } }
        public static string WhitePath { get { return @"..\..\..\Pente\Images\PenteWhiteCross.png"; } }

		[field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyStateChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TileState state = (TileState)value;

            string imageString = "";

            switch (state)
            {
                case TileState.EMPTY:
                    imageString = EmptyPath;
                    break;
                case TileState.WHITE:
                    imageString = WhitePath;
                    break;
                case TileState.BLACK:
                    imageString = BlackPath;
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
