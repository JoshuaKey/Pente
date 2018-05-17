using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pente
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Board board;

        public GameWindow(int width, int height)
        {
            InitializeComponent();
            board = GameManager.board;
            AddColumns(width);
            AddRows(height);
            AddButtons(width, height);
        }

        private void AddColumns(int columns)
        {
            grd_tiles.Columns = columns;
            for (int i = 0; i < columns; ++i)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(1, GridUnitType.Star);
                //grd_tiles.ColumnDefinitions.Add(cd);
            }
        }

        private void AddRows(int rows)
        {
            grd_tiles.Rows = rows;
            for (int i = 0; i < rows; ++i)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(1, GridUnitType.Star);
                //grd_tiles.RowDefinitions.Add(rd);
            }
        }

        private void AddButtons(int columns, int rows)
        {
            for (int i = 0; i < columns; ++i)
            {
                for (int j = 0; j < rows; ++j)
                {
                    Image b = new Image();
                    Piece p = new Piece();
                    p.TileState = TileState.EMPTY;
                    b.DataContext = p;
                    board.tiles[i, j] = p;

                    Binding binding = new Binding("TileState");
                    binding.Converter = p;
                    b.SetBinding(Image.SourceProperty, binding);
                    
                    b.Width = double.NaN;
                    b.Height = double.NaN;
                    b.MouseDown += Button_Click;
                    Grid.SetColumn(b, i);
                    Grid.SetRow(b, j);
                    grd_tiles.Children.Add(b);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int column = Grid.GetColumn(sender as Image);
            int row = Grid.GetRow(sender as Image);
            string announcement;
            GameManager.PlacePiece(column, row, out announcement);
        }
    }
}
