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
    /// Interaction logic for ModeSelction.xaml
    /// </summary>
    public partial class ModeSelction : Window
    {
        private string p2Name;

        public ModeSelction()
        {
            InitializeComponent();
            p2Name = "";
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Initialize((int)sld_boardSize.Value);
            SetPlayerNames();
            GameWindow gw = new GameWindow();
            gw.Left = Left;
            gw.Top = 0;
            gw.Show();
            Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MenuController mc = new MenuController();
            mc.Left = Left;
            mc.Top = Top;
            mc.Show();
            Close();
        }

        private void Computer_Clicked(object sender, RoutedEventArgs e)
        {
            if (cbx_computer.IsChecked == true)
            {
                tbx_p2Name.IsEnabled = false;
                p2Name = tbx_p2Name.Text;
                tbx_p2Name.Text = "Computer";
            }
            else
            {
                tbx_p2Name.IsEnabled = true;
                tbx_p2Name.Text = p2Name;
            }
        }

        private void SetPlayerNames()
        {
			Player p1 = GameManager.player1;
			Player p2 = GameManager.player2;
            GameManager.SetPlayerNames(tbx_p1Name.Text, tbx_p2Name.Text);
        }
    }
}
