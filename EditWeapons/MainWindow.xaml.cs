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
using Titanfall2ModdingLibrary;

namespace EditWeapons
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        long ReturnedAddress = 0;
        Modder Mod;
        string Loadout { get { return EditWeapons.Properties.Resources.Loadout; } }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Write_Click(object sender, RoutedEventArgs e)
        {

            Mod = new Modder();

            if (ReturnedAddress != Mod.TestAddress(ReturnedAddress, "global function GetPilotLoadoutForCurrentMapSP"))
                ReturnedAddress = Mod.TestPointers(new Pointer[]
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
                    Mod.TrialOfFire,
                    Mod.TheArk,
                    Mod.TheFoldWeapon
                }, "global function GetPilotLoadoutForCurrentMapSP");

            while (ReturnedAddress == -1)
                ReturnedAddress = InsertAddressBox.Show();


            string Finished = Loadout.Replace("%PilotLoadoutPrimary%", Primary.Text)
                                     .Replace("%PilotLoadoutSecondary%", Secondary.Text)
                                     .Replace("%PilotLoadoutOrdnance%", Ordnance.Text)
                                     .Replace("%PilotLoadoutMelee%", Melee.Text)
                                     .Replace("%PilotLoadoutSpecial%", Special.Text);


            List<byte> Data = new List<byte>(Encoding.ASCII.GetBytes(Finished));
            Data.Add(0);
            Mod.WriteMemory(ReturnedAddress, Data.ToArray());

        }
    }
}
