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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool register = false;
        
        public MainWindow()
        {

            InitializeComponent();

            

            

            passwordBox_2.Visibility = Visibility.Collapsed;
            label_pass.Visibility = Visibility.Collapsed;
            ResizeMode = ResizeMode.NoResize;

        }
                


        private void button_register_Click(object sender, RoutedEventArgs e)
        {

            passwordBox_2.Visibility = Visibility.Visible;
            label_pass.Visibility = Visibility.Visible;


            if (register == true)
            {
                //check USername
                if (false)
                {
                    //open MAinWINDOW
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Invalid Username");
                }
                //register
            }
            register = true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (register)
            {
                register = false;
                passwordBox_2.Visibility = Visibility.Collapsed;
                label_pass.Visibility = Visibility.Collapsed;
            }
            else
            {
                //log IN
                if (false);
                else
                {
                    MessageBoxResult result = MessageBox.Show("Invalid Username/Password combination");
                }
            }
        }
    }
}
