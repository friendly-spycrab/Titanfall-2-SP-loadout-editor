using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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


        /// <summary>
        /// Hardcoded pointer list to sp_loadouts file
        /// </summary>
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

        /// <summary>
        /// Gets memory at address
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public byte[] GetMemory(long Address,int Length)
        {
            byte[] buffer = new byte[Length];
            int bytesRead = 0;

            ReadProcessMemory((int)processHandle, Address, buffer, buffer.Length, ref bytesRead);

            return buffer;
        }

        /// <summary>
        /// Writes data into memory. and automatically add a null at the end
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Data"></param>
        /// <returns>returns number of bytes written</returns>
        public int WriteMemory(long Address,byte[] Data)
        {
            List<byte> Pad = new List<byte>(Data);
            Pad.Add(0);
            Data = Pad.ToArray();
            int bytesWritten = 0;
            WriteProcessMemory((int)processHandle, Address, Data, Data.Length, ref bytesWritten);
            return bytesWritten;
        }

        public long GetAddressFromPointer(Pointer Lev)
        {
            return Pointer(Lev.BaseAddress,Lev.offsets,Lev.ModuleName);
        }

        public long findAddress(byte[] DataToFind)
        {
            return MemoryScan.findaddress(processHandle,DataToFind);

        }

        /// <summary>
        /// Returns the address of a pointer
        /// </summary>
        /// <param name="BaseOffset">The base offset</param>
        /// <param name="Offsets">FileOffsets</param>
        /// <param name="ModuleName">The name of the dll file. example: Launcher.dll</param>
        /// <returns>Returns the address</returns>
        public long Pointer(long BaseOffset, long[] Offsets,string ModuleName)
        {

            foreach (ProcessModule item in process.Modules)
            {
                if (item.ModuleName == ModuleName)
                {
                    BaseOffset += (long)item.BaseAddress;
                    BaseOffset = BitConverter.ToInt64(GetMemory(BaseOffset, 8), 0);
                    break;
                }
            }

            for (int i = 0; i < Offsets.Length; i++)
            {
                if(Offsets[i] > 0)
                    BaseOffset = BitConverter.ToInt64(GetMemory(BaseOffset + Offsets[i],8),0);
            }

            return BaseOffset;
        }
        

        /// <summary>
        /// Tests pointer and sees if they give the expected result. returns the first valid pointer
        /// </summary>
        /// <param name="Levels"></param>
        /// <param name="expectedresult"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the address if true. else returns -1
        /// </summary>
        public long TestAddress(long Address, string expectedresult)
        {
            string Result = Encoding.ASCII.GetString(GetMemory(Address, expectedresult.Length));
            if (Result == expectedresult)
                return Address;

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
