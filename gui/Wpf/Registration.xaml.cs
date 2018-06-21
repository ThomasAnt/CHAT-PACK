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

namespace Wpf
{
    /// <summary>
    /// Interaktionslogik für Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
            button_log_in.Click += button_log_in_Click;
        }

        private void button_register_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_log_in_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new Wpf.MainWindow();

            w.tBoxName.Text = tboxUser.Text;
            this.Close();
            w.Show();
            
        }
    }
}
