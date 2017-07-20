using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Diagnostics;
using Titanfall2ModdingLibrary;
using System.Globalization;

namespace Titanfall_2_modder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Modder Mod;

        public MainWindow()
        {
            InitializeComponent();
            Mod = new Modder();
        }

        private void GetTextAtAddress_Click(object sender, RoutedEventArgs e)
        {
            byte[] Result = Mod.GetMemory(Convert.ToInt64(Address.Text,16),int.Parse(AmountOfCharacters.Text));
            Memory.Text = Encoding.ASCII.GetString(Result);
        }

        private void WriteToAddress_Click(object sender, RoutedEventArgs e)
        {
            int Length = int.Parse(AmountOfCharacters.Text);

            List<byte> Pad = new List<byte>(Encoding.ASCII.GetBytes(Memory.Text));
            if (Pad.Count <= Length)
            {
                if (Pad.Count < Length)
                {
                    Pad.Add(0);
                }
                Mod.WriteMemory(Convert.ToInt64(Address.Text, 16), Pad.ToArray());

            }
            else
                System.Windows.Forms.MessageBox.Show("To many characters");
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            long ReturnedAddress = Mod.TestPointers(new Pointer[] 
            {
                Mod.BoomTown,
                Mod.BoomTownStart,
                Mod.BoomTownEnd,
                Mod.Sewers,
                Mod.BT_7274,
                Mod.TimeShiftStart,
                Mod.TimeShift,
                Mod.TimeShiftEnd,
                Mod.TheBeaconStart,
                Mod.TheBeacon,
                Mod.TheBeaconEnd,
                Mod.TrialOfFire
            },
            "global function GetPilotLoadoutForCurrentMapSP");
            Address.Text =  ReturnedAddress.ToString("X4");
        }

        


    }
}
