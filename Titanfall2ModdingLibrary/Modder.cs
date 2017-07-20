using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Titanfall2ModdingLibrary
{
    public class Modder
    {
        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;

        public readonly Pointer LatestLoadedVPKFiles = new Pointer() { offsets = new long[] { 0x98, 0x70, 0xf8, 0x588, 0x128 }, BaseAddress = 0x14057B30, ModuleName = "engine.dll" };



        #region Maps
        public readonly Pointer BT_7274 = new Pointer() { offsets = new long[] { 0x28,0x240,0x2b00, 0x00 }, BaseAddress = 0x00080F70, ModuleName = "launcher.dll" };
        public readonly Pointer Sewers = new Pointer() { offsets = new long[] { 0xc80,0x00 }, BaseAddress = 0x13F0A418, ModuleName = "engine.dll" };
        public readonly Pointer BoomTownStart = new Pointer() { offsets = new long[] { 0x30, 0x68, 0x240, 0x28a0, 0x00 }, BaseAddress = 0x00080F50, ModuleName = "launcher.dll" };
        public readonly Pointer BoomTown = new Pointer() { offsets = new long[] { 0x50, 0x48, 0x240, 0x2E80, 0x00 }, BaseAddress = 0x00080F50, ModuleName = "launcher.dll" };
        public readonly Pointer BoomTownEnd = new Pointer() { offsets = new long[] { 0x28, 0x240, 0x2aa0, 0x00 }, BaseAddress = 0x00080F70, ModuleName = "launcher.dll" };
        public readonly Pointer TimeShiftStart = new Pointer() { offsets = new long[] { 0x378, 0x1AE8, 0x240, 0x2D20,0x00 }, BaseAddress = 0x13B86908, ModuleName = "engine.dll" };
        public readonly Pointer TimeShift = new Pointer() { offsets = new long[] { 0x28,0x240, 0x3240,0x00 }, BaseAddress = 0x00080F70, ModuleName = "launcher.dll" };
        public readonly Pointer TimeShiftEnd = new Pointer() { offsets = new long[] { 0x50,0x48,0x240,0x2d20,0x00 }, BaseAddress = 0x00080F50, ModuleName = "launcher.dll" };
        public readonly Pointer TheBeaconStart = new Pointer() { offsets = new long[] { 0x50,0x48,0x240,0x2C60,0x00 }, BaseAddress = 0x00080F50, ModuleName = "launcher.dll" };
        public readonly Pointer TheBeacon = new Pointer() { offsets = new long[] { 0x50,0x48,0x240,0x2860,0x00 }, BaseAddress = 0x00080F50, ModuleName = "launcher.dll" };
        public readonly Pointer TheBeaconEnd = new Pointer() { offsets = new long[] { 0x4C8,0x4DD0,0x240,0x2C60,0x00 }, BaseAddress = 0x00BC9170, ModuleName = "client.dll" };
        public readonly Pointer TrialOfFire = new Pointer() { offsets = new long[] { 0x28,0x240,0x2e40,0x00}, BaseAddress = 0x00080F70, ModuleName = "launcher.dll" };
        public readonly Pointer TheArk = new Pointer() { offsets = new long[] {0x50,0x48,0x240,0x2820,0x00 }, BaseAddress = 0x00080F50, ModuleName = "launcher.dll" };
        public readonly Pointer TheFoldWeapon = new Pointer() { offsets = new long[] {0x50,0x48,0x240,0x32c0,0x00 }, BaseAddress = 0x00080F50, ModuleName = "launcher.dll" };
        #endregion



        IntPtr processHandle;
        Process process;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);


        public Modder(string ProccessName = "Titanfall2")
        {
            process = Process.GetProcessesByName(ProccessName)[0];
            processHandle = OpenProcess(PROCESS_WM_READ | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_QUERY_INFORMATION | MEM_COMMIT | PAGE_READWRITE, false, process.Id);

        }

        public byte[] GetMemory(long Address,int Length)
        {
            byte[] buffer = new byte[Length];
            int bytesRead = 0;

            ReadProcessMemory((int)processHandle, Address, buffer, buffer.Length, ref bytesRead);

            return buffer;
        }

        public int WriteMemory(long Address,byte[] Data)
        {
            int bytesWritten = 0;
            WriteProcessMemory((int)processHandle, Address, Data, Data.Length, ref bytesWritten);
            return bytesWritten;
        }

        public long GetAddressFromPointer(Pointer Lev)
        {
            return Pointer(Lev.BaseAddress,Lev.offsets,Lev.ModuleName);
        }

        public long Pointer(long Address, long[] Offsets,string ModuleName)
        {

            foreach (ProcessModule item in process.Modules)
            {
                if (item.ModuleName == ModuleName)
                {
                    Address += (long)item.BaseAddress;
                    Address = BitConverter.ToInt64(GetMemory(Address, 8), 0);
                    break;
                }
            }
            for (int i = 0; i < Offsets.Length; i++)
            {
                if(Offsets[i] > 0)
                    Address = BitConverter.ToInt64(GetMemory(Address + Offsets[i],8),0);
            }

            return Address;
        }
        
        public long TestPointers(Pointer[] Levels,string expectedresult)
        {
            foreach (Pointer item in Levels)
            {
                string Result = Encoding.ASCII.GetString(GetMemory(GetAddressFromPointer(item), expectedresult.Length));
                if (Result == expectedresult)
                    return GetAddressFromPointer(item);
            }

            return -1;

        }

    }

    public struct Pointer
    {
        public long[] offsets;
        public long BaseAddress;
        public string ModuleName;
    }
}
