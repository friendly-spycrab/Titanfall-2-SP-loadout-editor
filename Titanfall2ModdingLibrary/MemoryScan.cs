using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Titanfall2ModdingLibrary
{
    public static class MemoryScan
    {

        // REQUIRED CONSTS

        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;
        const int PROCESS_WM_READ = 0x0010;

        // REQUIRED METHODS
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, long dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess,IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);


        // REQUIRED STRUCTS

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }


        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }



        public static long findaddress(IntPtr Handle,byte[] DataToFind)
        {

            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            long proc_min_address = (long)sys_info.minimumApplicationAddress;
            long proc_max_address = (long)sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later



            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            int bytesRead = 0;
            while (proc_min_address < proc_max_address)
            {
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                VirtualQueryEx(Handle, (IntPtr)proc_min_address, out mem_basic_info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
                
                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    byte[] buffer = new byte[(long)mem_basic_info.RegionSize];

                    // read everything in the buffer above
                    ReadProcessMemory((int)Handle, (long)mem_basic_info.BaseAddress, buffer, (long)mem_basic_info.RegionSize, ref bytesRead);


                    long Index = SimpleBoyerMooreSearch(buffer,DataToFind);
                    if (Index != -1)
                        return Index + proc_min_address;
                }

                // move to the next memory chunk
                proc_min_address += (long)mem_basic_info.RegionSize;
            }
            return -1;
        }

        private static long SimpleBoyerMooreSearch(byte[] haystack, byte[] needle)
        {
            unchecked
            {


                int[] lookup = new int[256];
                for (int i = 0; i < lookup.Length; i++) { lookup[i] = needle.Length; }

                for (int i = 0; i < needle.Length; i++)
                {
                    lookup[needle[i]] = needle.Length - i - 1;
                }

                long index = needle.Length - 1;
                var lastByte = needle.Last();
                while (index < haystack.Length)
                {
                    var checkByte = haystack[index];
                    if (haystack[index] == lastByte)
                    {
                        bool found = true;
                        for (int j = needle.Length - 2; j >= 0; j--)
                        {
                            if (haystack[index - needle.Length + j + 1] != needle[j])
                            {
                                found = false;
                                break;
                            }
                        }

                        if (found)
                            return index - needle.Length + 1;
                        else
                            index++;
                    }
                    else
                    {
                        index += lookup[checkByte];
                    }
                }
            }
            return -1;
        }

    }
}
