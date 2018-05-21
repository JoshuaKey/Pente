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
using System.Windows.Threading;

namespace Pente
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Board board;
		private DispatcherTimer timer { get; } = new DispatcherTimer();
		private double currentTime = 0.0;
		public GameWindow()
		{
			InitializeComponent();
			board = GameManager.board;
			grd_tiles.Columns = board.Width;
			grd_tiles.Rows = board.Height;
			AddButtons(board.Width, board.Height);
			tbl_announcement.Text = "";
			lbl_playerTurn.Content = GameManager.GetCurrentPlayer().name + "'s";

			timer.Tick += Timer_Tick;
			timer.Interval = new TimeSpan(0, 0, 1);
			timer.Start();
		}
		private void Window_Close(object sender, EventArgs e)
		{
			timer.Stop();
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
			currentTime = 0.0;
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
		private void Save_Click(object sender, RoutedEventArgs e)
		{
			timer.Stop();
			GameManager.Save();
			timer.Start();
		}
		private void Timer_Tick(object sender, EventArgs e)
		{
			currentTime += timer.Interval.TotalMilliseconds;
			if (currentTime >= 20000.0)
			{
				currentTime = 0.0;
				GameManager.player1Turn = !GameManager.player1Turn;
				tbl_announcement.Text = "";
				lbl_playerTurn.Content = GameManager.GetCurrentPlayer().name + "'s";
			}
			lbl_timer.Content = (20.0 - currentTime / 1000).ToString();
		}
    }
}
