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

namespace Wpf
{
    /// <summary>
    /// Interaction logic for RequestWin.xaml
    /// </summary>
    public partial class RequestWin : Window
    {
        public RequestWin()
        {
            InitializeComponent();

            BtnAccept.Click += AcceptRequest;
            BtnDecline.Click += DeclineRequest;
        }

        private void DeclineRequest(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AcceptRequest(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
