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
            tbl_announcement.Text = "";
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
                    Image image = new Image();
                    Piece p = board.tiles[i, j];
                    image.DataContext = p;

                    Binding binding = new Binding("TileState");
                    binding.Converter = p;
                    image.SetBinding(Image.SourceProperty, binding);

                    image.Width = double.NaN;
                    image.Height = double.NaN;
                    image.MouseDown += Button_Click;
                    Grid.SetColumn(image, i);
                    Grid.SetRow(image, j);
                    grd_tiles.Children.Add(image);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int column = Grid.GetColumn(sender as Image);
            int row = Grid.GetRow(sender as Image);
            string announcement;
            GameManager.PlacePiece(column, row, out announcement);
            tbl_announcement.Text = string.IsNullOrEmpty(announcement) ? "" : announcement;
            lbl_playerTurn.Content = GameManager.GetCurrentPlayer().name + "'s";
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuController mc = new MenuController();
            mc.Left = Left;
            mc.Top = 20;
            mc.Show();
            Close();
        }
    }
}
