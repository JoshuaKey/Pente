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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pente {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuController : Window {
        public MenuController() {
            InitializeComponent();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            ModeSelction ms = new ModeSelction();
            ms.Show();
            Close();
        }

        private void Rules_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
