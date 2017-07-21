using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
namespace EditWeapons
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                File.WriteAllText("Error.txt",ex.ToString());
                System.Windows.Forms.MessageBox.Show("sorry an exception occured");
                Environment.Exit(-1);
            }
            // here you take control
        }
    }
}
