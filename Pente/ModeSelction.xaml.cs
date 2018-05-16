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
        public ModeSelction()
        {
            InitializeComponent();
            GameManager.Initialize();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Computer_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SetPlayerNames()
        {
            GameManager.player1.name = tbx_p1Name.Text;
            GameManager.player2.name = tbx_p2Name.Text;
        }
    }
}
