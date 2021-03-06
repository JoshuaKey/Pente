﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class MenuController : Page {
        public MenuController() {
            InitializeComponent();
            
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Page p = new ModeSelction();
            NavigationService.Navigate(p);
        }

        private void Rules_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.pente.net/instructions.html");
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            bool success = GameManager.Load();
            if (success)
            {
                Page p = new GameWindow();
                NavigationService.Navigate(p);
            }
        }
    }
}
