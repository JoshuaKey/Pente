using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool locked;
		private DispatcherTimer timer { get; } = new DispatcherTimer();
		private double currentTime = 0.0;
		public GameWindow()
		{
			InitializeComponent();
			board = GameManager.board;
			grd_tiles.Columns = board.Width;
			grd_tiles.Rows = board.Height;
			AddButtons(board.Width, board.Height);
			lbl_playerTurn.Content = GameManager.GetCurrentPlayer().name + "'s";
            locked = false;
            tbl_announcement.Text = "";

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
            string baseText = GameManager.GetCurrentPlayer().name;
            bool placed = false;
            announcement = "";
            if (!locked) placed = GameManager.PlacePiece(column, row, out announcement);
            if (GameManager.GetCurrentPlayer().isComputer) locked = true;
            else locked = false;
            string text = GetAnnouncmentText(announcement, baseText);
            if (GameManager.BoardLocked)
            {
                timer.Stop();
            }
            else if (placed)
            {
                timer.Start();
                currentTime = 0.0f;
            }
            lbl_playerTurn.Content = GameManager.GetCurrentPlayer().name + "'s";
            if (placed || text != "") tbl_announcement.Text = text;
            lbl_captures.Content = GameManager.GetCurrentPlayer().captures;
            if (placed && GameManager.GetCurrentPlayer().isComputer)
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Thread.Sleep(800);
                    baseText = GameManager.GetCurrentPlayer().name;
                    GameManager.MakeComputerMove(out announcement);
                    text = GetAnnouncmentText(announcement, baseText);
                    Dispatcher.Invoke(() =>
                    {
                        lbl_playerTurn.Content = GameManager.GetCurrentPlayer().name + "'s";
                        tbl_announcement.Text = text;
                    });
                    locked = false;
                }).Start();
            }
        }

        private string GetAnnouncmentText(string announcement, string baseText)
        {
            string text = baseText;
            switch (announcement)
            {
                case "Capture":
                    text += " captured enough pieces to win the game!";
                    break;
                case "Pente":
                    text += " got five in a row to win the game!";
                    break;
                case "Tessera":
                    text += " made a tessera!";
                    break;
                case "Tria":
                    text += " made a tria!";
                    break;
                case "Second":
                    text += ", it must be at least 3 away";
                    break;
                default:
                    int cap;
                    if (!string.IsNullOrEmpty(announcement))
                    {
                        int.TryParse(announcement[announcement.Length - 1] + "", out cap);
                        if (cap > 0)
                        {
                            text += $" made {cap} capture";
                            if (cap > 1) text += "s";
                        }
                        else
                        {
                            text = "";
                        }
                    }
                    else
                    {
                        text = "";
                    }

                    break;
            }
            
            return text;
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
                timer.Stop();
                TimerTurnSwitch tts = new TimerTurnSwitch(GameManager.GetCurrentPlayer().name + ", you ran out of time and the turn switched.");
                tts.Top = Top + Height / 2 - tts.Height / 2;
                tts.Left = Left + Width / 2 - tts.Width / 2;
                tts.ShowDialog();
                currentTime = 0.0;
                GameManager.SwitchPlayerTurn();
                tbl_announcement.Text = "";
                lbl_playerTurn.Content = GameManager.GetCurrentPlayer().name + "'s";
                timer.Start();
            }
			lbl_timer.Content = (20.0 - currentTime / 1000).ToString();
		}
    }
}
