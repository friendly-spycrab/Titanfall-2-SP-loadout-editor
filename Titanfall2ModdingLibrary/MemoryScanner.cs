using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Titanfall2ModdingLibrary
{
    public class MemoryScanner
    {
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress,out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, long lpBaseAddress,byte[] lpBuffer, long dwSize, ref int lpNumberOfBytesRead);

        IntPtr processHandle;
        long proc_min_address;
        long proc_max_address;

        public MemoryScanner(IntPtr Handle)
        {
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);
            processHandle = Handle;
            proc_min_address = (long)sys_info.minimumApplicationAddress;
            proc_max_address = (long)sys_info.maximumApplicationAddress;


        }

        public long Scan(byte[] pattern)
        {
            long Address = 0;
            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

            int bytesRead = 0;  // number of bytes read with ReadProcessMemory
            while (proc_min_address < proc_max_address)
            {
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                VirtualQueryEx(processHandle, (IntPtr)proc_min_address, out mem_basic_info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    byte[] buffer = new byte[(uint)mem_basic_info.RegionSize];

                    // read everything in the buffer above
                    ReadProcessMemory((int)processHandle,(long)mem_basic_info.BaseAddress, buffer, (long)mem_basic_info.RegionSize, ref bytesRead);

                    // then output this in the file
                    Parallel.For(0,(uint)mem_basic_info.RegionSize - pattern.Length + 1, i =>
                    {
                        if (!((long)mem_basic_info.BaseAddress + i + pattern.Length > (long)mem_basic_info.RegionSize + (long)mem_basic_info.BaseAddress))
                            if (pattern == buffer.ToList().GetRange((int)i, pattern.Length).ToArray())
                                Address = (long)mem_basic_info.BaseAddress + i;
                    });

                    if (Address != 0)
                        return Address;
                }

                // move to the next memory chunk
                proc_min_address += (long)mem_basic_info.RegionSize;
            }
            return 0;

        }

    }

    public struct SYSTEM_INFO
    {
        public ushort processorArchitecture;
        ushort reserved;
        public uint pageSize;
        public IntPtr minimumApplicationAddress;  // minimum address
        public IntPtr maximumApplicationAddress;  // maximum address
        public IntPtr activeProcessorMask;
        public uint numberOfProcessors;
        public uint processorType;
        public uint allocationGranularity;
        public ushort processorLevel;
        public ushort processorRevision;
    }

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


}
