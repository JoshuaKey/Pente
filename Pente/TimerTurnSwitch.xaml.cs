﻿using System;
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
    /// Interaction logic for TimerTurnSwitch.xaml
    /// </summary>
    public partial class TimerTurnSwitch : Window
    {
        public TimerTurnSwitch(string alertMessage)
        {
            InitializeComponent();
            tbl_alert.Text = alertMessage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
