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

namespace EditWeapons
{
    /// <summary>
    /// Interaction logic for InsertAddress.xaml
    /// </summary>
    public partial class InsertAddress : Window
    {
        public InsertAddress()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(-1);
        }
    }

    public static class InsertAddressBox
    {
        public static long Show()
        {
            InsertAddress i = new InsertAddress();
            i.ShowDialog();
            try
            {
                return Convert.ToInt64(i.Address.Text, 16);
            }
            catch (Exception)
            {

                return -1;
            }
        }
    }
}
