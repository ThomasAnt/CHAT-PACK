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
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System.Windows.Media.Animation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for demoExample.xaml
    /// </summary>
    public partial class demoExample : Window
    {
        private delegate void ChangeWin(string text);
        public demoExample()
        {
            InitializeComponent();
            //Window a = new Wpf.MainWindow();

            //a.ShowDialog();

            //string connectionString = @"server = (localdb)\MSSQLLocalDB";

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    try
            //    {
            //        SqlCommand command = new SqlCommand("SELECT * FROM  tablename WHERE STATID = @param", connection);
            //        connection.Open();

            //        //Prepare command
            //        command.Parameters.AddWithValue("@param", );

            //        //Execute Reader / The Query
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                // Display all the columns. 
            //                for (int i = 0; i < reader.FieldCount; i++)
            //                {
            //                    //(int)reader.GetValue(i) 
            //                }
            //            }
            //        }

            //        ...

            //    }
            //    catch (SqlException ex)
            //    {
            //        ...
            //    }
            //} (edited)
            this.FontSize = 18;
            //Process[] processlist = Process.GetProcesses();

            

            Thread t = new Thread(GetTitle);
            t.IsBackground = true;

            t.Start();

            //foreach (Process process in processlist)
            //{
            //    if (!String.IsNullOrEmpty(process.MainWindowTitle))
            //    {
            //        string t = "Process: " + process.ProcessName + "     ID: " + process.Id + "      Window title: "+ process.MainWindowTitle +"\n";
            //        tb.Text += t;
            //    }
            //}


            //pop.IsOpen = true;
        }

        private void GetTitle()
        {
            Process p;
            while (true)
            {
                p = Process.GetCurrentProcess();
                ChangeWin del = new ChangeWin(ChangeText);
                Thread.Sleep(700);
                tb.Dispatcher.BeginInvoke(del, p.ProcessName);
            }
            
        }

        private void ChangeText(string text)
        {
            tb.Text = text;
        }
    }
}
