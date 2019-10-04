using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MemoryReader
{
    public class InstanceController
    {
        private static string AppMutexName = "{65FBE93E-CF8B-4930-AEF1-C6463031446F}";
        private static Mutex mutex;
        public static bool IsSingleInstance()
        {
            mutex = new Mutex(false, AppMutexName);
            return mutex.WaitOne(0, false);
        }
    }

    public class PointerChainWrap
    {
		public static short GetShortVal(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
		{
			var pc = new PointerChain<short>(procId, basePtr, offsetArray);
			return pc.getValue(int32);
		}

		public static short GetShortVal(IntPtr handle, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
		{
			var pc = new PointerChain<short>(0, basePtr, offsetArray);
			return pc.GetValueV2(handle, int32);
		}

		public static uint GetUintVal(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<uint>(procId, basePtr, offsetArray);
            return pc.getValue(int32);
        }

        public static ulong GetUint64Val(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<ulong>(procId, basePtr, offsetArray);
            return pc.getValue(int32);
        }

        public static uint GetUintVal(IntPtr handle, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<uint>(0, basePtr, offsetArray);
            return pc.GetValueV2(handle, int32);
        }

        public static ulong GetUint64Val(IntPtr handle, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<ulong>(0, basePtr, offsetArray);
            return pc.GetValueV2(handle, int32);
        }

        //public static uint GetUintVal32(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        //{
        //    var pc = new PointerChain<uint>(procId, basePtr, offsetArray);
        //    return pc.getValue32(int32);
        //}

        public static uint GetByteVal(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<byte>(procId, basePtr, offsetArray);
            return pc.getValue(int32);
        }

        public static float GetFloatVal(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<float>(procId, basePtr, offsetArray);
            return pc.getValue(int32);
        }

        public static uint GetByteVal(IntPtr handle, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<byte>(0, basePtr, offsetArray);
            return pc.GetValueV2(handle, int32);
        }

        public static float GetFloatVal(IntPtr handle, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<float>(0, basePtr, offsetArray);
            return pc.GetValueV2(handle, int32);
        }

        public static UInt64 GetPtr(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<uint>(procId, basePtr, offsetArray);
            return pc.getPtr(int32);
        }
        public static UInt64 GetPtrV2(IntPtr handle, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
        {
            var pc = new PointerChain<uint>(0, basePtr, offsetArray);
            return pc.GetPtrV2(handle, int32);
        }

        public static uint GetWordVal(uint procId, UInt64 basePtr, uint[] offsetArray)
        {
            var pc = new PointerChain<uint>(procId, basePtr, offsetArray);
            return pc.GetWord(basePtr);
        }
    }


    //public class PointerChainWrap
    //{
    //    public static uint GetUintVal(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
    //    {
    //        var pc = new PointerChain<uint>(procId, basePtr, offsetArray);
    //        return pc.getValue(int32);
    //    }

    //    public static uint GetByteVal(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
    //    {
    //        var pc = new PointerChain<byte>(procId, basePtr, offsetArray);
    //        return pc.getValue(int32);
    //    }

    //    public static float GetFloatVal(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
    //    {
    //        var pc = new PointerChain<float>(procId, basePtr, offsetArray);
    //        return pc.getValue(int32);
    //    }

    //    public static UInt64 GetPtr(uint procId, UInt64 basePtr, uint[] offsetArray, bool int32 = false)
    //    {
    //        var pc = new PointerChain<uint>(procId, basePtr, offsetArray);
    //        return pc.getPtr(int32);
    //    }
    //}


    public class PointerChain<T> where T : IConvertible
    {
        private UInt64 m_basePtr;
        private uint[] m_offsetArray;
        private uint m_nProcId;

        public PointerChain(uint procId, UInt64 basePtr, uint[] offsetArray)
        {
            m_nProcId = procId;
            m_basePtr = basePtr;
            m_offsetArray = offsetArray; //??  throw (new ArgumentException());
        }

        private UInt64 GetUint(UInt64 addr, bool int32 = false)
        {
            if (int32)
            {
                byte[] bytes = Utils.ReadByteFromProcess(m_nProcId, addr, 4);
                if (bytes == null || bytes.Length != 4) throw (new InvalidOperationException());
                return BitConverter.ToUInt32(bytes, 0);
            }
            else
            {
                byte[] bytes = Utils.ReadByteFromProcess(m_nProcId, addr, 8);
                if (bytes == null || bytes.Length != 8) throw (new InvalidOperationException());
                return BitConverter.ToUInt64(bytes, 0);
            }

        }

        private UInt64 GetUintV2(IntPtr processHandle, UInt64 addr, bool int32 = false)
        {
            if (int32)
            {
                var bytes = Utils.ReadByteFromProcess(processHandle, addr, 4);
                if (bytes == null || bytes.Length != 4) throw (new InvalidOperationException());
                return BitConverter.ToUInt32(bytes, 0);
            }
            else
            {
                var bytes = Utils.ReadByteFromProcess(processHandle, addr, 8);
                if (bytes == null || bytes.Length != 8) throw (new InvalidOperationException());
                return BitConverter.ToUInt64(bytes, 0);
            }

        }

        //private UInt64 GetUint(UInt64 addr)
        //{
        //    byte[] bytes = Utils.ReadByteFromProcess(m_nProcId, addr, 8);
        //    if (bytes == null || bytes.Length != 8) throw (new InvalidOperationException());

        //    return BitConverter.ToUInt64(bytes, 0);
        //}

        //private UInt32 GetUint32(UInt32 addr)
        //{
        //    byte[] bytes = Utils.ReadByteFromProcess(m_nProcId, addr, 4);
        //    if (bytes == null || bytes.Length != 4) throw (new InvalidOperationException());

        //    return (UInt32)BitConverter.ToInt32(bytes, 0);
        //}


        private T GetTVal(UInt64 addr)
        {
            uint sizeoft = (uint)Marshal.SizeOf(typeof(T));
            byte[] bytes = Utils.ReadByteFromProcess(m_nProcId, addr, sizeoft);
            if (bytes == null || bytes.Length != sizeoft) throw (new InvalidOperationException());

            if (typeof(T) == typeof(uint))
            {
                return (T)Convert.ChangeType(BitConverter.ToUInt32(bytes, 0), typeof(T));
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)Convert.ChangeType(BitConverter.ToSingle(bytes, 0), typeof(T));
            }
            else if (typeof (T) == typeof (ulong))
            {
                return (T) Convert.ChangeType(BitConverter.ToUInt64(bytes, 0), typeof (T));
            }
			else if (typeof(T) == typeof(short))
			{
				return (T)Convert.ChangeType(BitConverter.ToInt16(bytes, 0), typeof(T));
			}
			
			else throw (new ArgumentException());
        }

        private T GetTValV2(IntPtr handle, ulong addr)
        {
            uint sizeoft = (uint)Marshal.SizeOf(typeof(T));
            byte[] bytes = Utils.ReadByteFromProcess(handle, addr, sizeoft);
            if (bytes == null || bytes.Length != sizeoft) throw (new InvalidOperationException());

            if (typeof(T) == typeof(uint))
            {
                return (T)Convert.ChangeType(BitConverter.ToUInt32(bytes, 0), typeof(T));
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)Convert.ChangeType(BitConverter.ToSingle(bytes, 0), typeof(T));
            }
            else if (typeof (T) == typeof (ulong))
            {
                return (T) Convert.ChangeType(BitConverter.ToUInt64(bytes, 0), typeof (T));
            }
			else if (typeof(T) == typeof(short))
			{
				return (T)Convert.ChangeType(BitConverter.ToInt16(bytes, 0), typeof(T));
			}
			
			else throw (new ArgumentException());
        }

        public T getValue(bool int32)
        { 
            if (!m_offsetArray.Any()) return GetTVal(m_basePtr);
            var curval = GetUint(m_basePtr, int32);
            for (uint i = 0; i < m_offsetArray.Length - 1; ++i)
            {
                curval = GetUint(curval + m_offsetArray[i], int32);
            }
            return GetTVal(curval + m_offsetArray[m_offsetArray.Length - 1]);
        }

        public T GetValueV2(IntPtr handle, bool int32)
        { 
            if (!m_offsetArray.Any()) return GetTValV2(handle, m_basePtr);
            var curval = GetUintV2(handle, m_basePtr, int32);
            for (uint i = 0; i < m_offsetArray.Length - 1; ++i)
            {
                curval = GetUintV2(handle, curval + m_offsetArray[i], int32);
            }
            return GetTValV2(handle, curval + m_offsetArray[m_offsetArray.Length - 1]);
        }

        //public T getValue32()
        //{
        //    UInt32 curval = (UInt32)GetUint(m_basePtr, true);

        //    for (uint i = 0; i < m_offsetArray.Length - 1; ++i)
        //    {
        //        curval = (UInt32)GetUint(curval + m_offsetArray[i], true);
        //    }
        //    return GetTVal(curval + m_offsetArray[m_offsetArray.Length - 1]);
        //}

        public UInt64 getPtr(bool int32)
        {
            UInt64 curval = GetUint(m_basePtr, int32);

            for (uint i = 0; i < m_offsetArray.Length - 1; ++i)
            {
                curval = GetUint(curval + m_offsetArray[i], int32);
            }
            return curval + m_offsetArray[m_offsetArray.Length - 1];
        }

        public UInt64 GetPtrV2(IntPtr processHandle, bool int32)
        {
            UInt64 curval = GetUintV2(processHandle, m_basePtr, int32);

            for (uint i = 0; i < m_offsetArray.Length - 1; ++i)
            {
                curval = GetUintV2(processHandle, curval + m_offsetArray[i], int32);
            }
            return curval + m_offsetArray[m_offsetArray.Length - 1];
        }

        public T getValue16()
        {
            UInt32 curval = GetWord(m_basePtr);

            for (uint i = 0; i < m_offsetArray.Length - 1; ++i)
            {
                curval = GetWord(curval + m_offsetArray[i]);
            }
            return GetTVal(curval + m_offsetArray[m_offsetArray.Length - 1]);
        }

        public UInt32 GetWord(UInt64 addr)
        {
            byte[] bytes = Utils.ReadByteFromProcess(m_nProcId, addr, 2);
            if (bytes == null || bytes.Length != 2) throw (new InvalidOperationException());
            return (UInt32)BitConverter.ToInt16(bytes, 0);
        }

	
	}


    namespace Read64bitRegistryFrom32bitApp
    {
        public enum RegSAM
        {
            QueryValue = 0x0001,
            SetValue = 0x0002,
            CreateSubKey = 0x0004,
            EnumerateSubKeys = 0x0008,
            Notify = 0x0010,
            CreateLink = 0x0020,
            WOW64_32Key = 0x0200,
            WOW64_64Key = 0x0100,
            WOW64_Res = 0x0300,
            Read = 0x00020019,
            Write = 0x00020006,
            Execute = 0x00020019,
            AllAccess = 0x000f003f
        }

        public static class RegHive
        {
            public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);
            public static UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001u);
        }

        public static class RegistryWOW6432
        {
            #region Member Variables
            #region Read 64bit Reg from 32bit app
            [DllImport("Advapi32.dll")]
            static extern uint RegOpenKeyEx(
                UIntPtr hKey,
                string lpSubKey,
                uint ulOptions,
                int samDesired,
                out int phkResult);

            [DllImport("Advapi32.dll")]
            static extern uint RegCloseKey(int hKey);

            [DllImport("advapi32.dll", EntryPoint = "RegQueryValueEx")]
            public static extern int RegQueryValueEx(
                int hKey, string lpValueName,
                int lpReserved,
                ref uint lpType,
                System.Text.StringBuilder lpData,
                ref uint lpcbData);
            #endregion
            #endregion

            #region Functions
            static public string GetRegKey64(UIntPtr inHive, String inKeyName, String inPropertyName)
            {
                return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_64Key, inPropertyName);
            }

            static public string GetRegKey32(UIntPtr inHive, String inKeyName, String inPropertyName)
            {
                return GetRegKey64(inHive, inKeyName, RegSAM.WOW64_32Key, inPropertyName);
            }

            static public string GetRegKey64(UIntPtr inHive, String inKeyName, RegSAM in32or64key, String inPropertyName)
            {
                //UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
                int hkey = 0;

                try
                {
                    uint lResult = RegOpenKeyEx(RegHive.HKEY_LOCAL_MACHINE, inKeyName, 0, (int)RegSAM.QueryValue | (int)in32or64key, out hkey);
                    if (0 != lResult) throw new Exception(string.Format("Not found {0}\\{1} reg key", inKeyName, inPropertyName));
                    uint lpType = 0;
                    uint lpcbData = 1024;
                    StringBuilder AgeBuffer = new StringBuilder(1024);
                    RegQueryValueEx(hkey, inPropertyName, 0, ref lpType, AgeBuffer, ref lpcbData);
                    string Age = AgeBuffer.ToString();
                    return Age;
                }
                finally
                {
                    if (0 != hkey) RegCloseKey(hkey);
                }
            }
            #endregion

            #region Enums
            #endregion
        }
    }

    public static class Utils
    {
        static public bool ArraysEqual(byte[] a1, int start, byte[] a2)
        {
            if (Math.Min(a1.Length - start, a2.Length) == a2.Length)
            {
                for (int i = 0; i < a2.Length; i++)
                {
                    if (a1[i + start] != a2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        static public string Fifa13GameFolder()
        {
            return Read64bitRegistryFrom32bitApp.RegistryWOW6432.GetRegKey32(Read64bitRegistryFrom32bitApp.RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\EA Sports\\FIFA 13", "Install Dir");
        }

        static public string Fifa15GameFolder()
        {
            return Read64bitRegistryFrom32bitApp.RegistryWOW6432.GetRegKey32(Read64bitRegistryFrom32bitApp.RegHive.HKEY_LOCAL_MACHINE, "SOFTWARE\\EA Sports\\FIFA 15", "Install Dir");
        }

        public static readonly uint PROCESS_QUERY_INFORMATION = 0x0400;
        public static readonly uint PROCESS_VM_READ = 0x0010;

        public const int PROCESS_VM_WRITE = 0x0020;
        public const int PROCESS_VM_OPERATION = 0x0008;



        //		BOOL WriteProcessMemory(
        //			HANDLE hProcess,                // handle to process
        //			LPVOID lpBaseAddress,           // base of memory area
        //			LPCVOID lpBuffer,               // data buffer
        //			SIZE_T nSize,                   // count of bytes to write
        //			SIZE_T * lpNumberOfBytesWritten // count of bytes written
        //			);
        //[DllImport("kernel32.dll")]
        //public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
        public static bool WriteMemory(Process process, int address, int lenght, byte[] buffer)
        {
            try
            {
                IntPtr processHandle = OpenProcess(0x1F0FFF, false, Convert.ToUInt32(process.Id));
                int bytesWritten = 0;
                // '\0' marks the end of string
                WriteProcessMemory((int)processHandle, address, buffer, lenght, ref bytesWritten);
                return true;
            }
            catch { return false; }
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, ref UInt32 lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, ref UInt32 lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
        [DllImport("kernel32.dll")]
        static extern void GetNativeSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetExitCodeProcess(IntPtr hProcess, ref Int32 lpExitCode);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool IsWow64Process(IntPtr hProcess, ref bool Wow64Process);

        /*  private static Encoding ae = Encoding.GetEncoding(
                "utf-8",
                new EncoderExceptionFallback(),
                new DecoderExceptionFallback());*/

        private static Encoding au = Encoding.GetEncoding(
              "utf-16",
              new EncoderExceptionFallback(),
              new DecoderExceptionFallback());

      

         

      



        public static string GetStringByAddress(uint procId, UInt64 address, out bool endOfString, out uint procBytes, uint size = 0x250)
        {
            endOfString = false;
            procBytes = 0;
            byte[] buffGaolMaker = Utils.ReadByteFromProcess(procId, address, size);
            if (buffGaolMaker == null || buffGaolMaker.Length == 0) return "";

            int byteCount = buffGaolMaker.Length;
            for (int i = 0; i < buffGaolMaker.Length; ++i)
            {
                if (i % 2 == 1 && i != 0 && buffGaolMaker[i - 1] == 0 && buffGaolMaker[i] == 0)
                {
                    byteCount = i - 1;
                    procBytes = (uint)byteCount + 1;
                    break;
                }
            }

            try
            {
                return au.GetString(buffGaolMaker, 0, byteCount);
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }

        private static byte[] ReadByteFromProccessHandle(IntPtr handle, UIntPtr address, UInt32 size, ref UInt32 bytes)
        {
            if (size == 0) return null;
            byte[] buffer = new byte[size];
            if (ReadProcessMemory(handle, address, buffer, size, ref bytes) == false) return null;
            return buffer;
        }

        public static byte[] ReadByteFromProcess(uint processId, uint pos, uint size)
        {
            IntPtr hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, processId);
            if (hProc == IntPtr.Zero) return null;
            UInt32 nReadBytes = 0;
            byte[] rbuff = ReadByteFromProccessHandle(hProc, (UIntPtr)pos, (UInt32)size, ref nReadBytes);
            CloseHandle(hProc);
            return rbuff;
        }

        public static byte[] ReadByteFromProcess(uint processId, UInt64 pos, uint size)
        {
            IntPtr hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, processId);
            if (hProc == IntPtr.Zero) return null;
            UInt32 nReadBytes = 0;
            byte[] rbuff = ReadByteFromProccessHandle(hProc, (UIntPtr)pos, (UInt32)size, ref nReadBytes);
            CloseHandle(hProc);
            return rbuff;
        }

        public static byte[] ReadByteFromProcess(IntPtr processHandle, UInt64 pos, uint size)
        {
            if (processHandle == IntPtr.Zero) return null;
            uint nReadBytes = 0;
            var rbuff = ReadByteFromProccessHandle(processHandle, (UIntPtr)pos, (UInt32)size, ref nReadBytes);
            return rbuff;
        }

        public static UInt32 ReadDwordFromProcess(uint processId, UInt64 addr)
        {
            byte[] bytes = ReadByteFromProcess(processId, addr, 4);
            if (bytes == null || bytes.Length != 4) throw (new InvalidOperationException());
            return (UInt32)BitConverter.ToUInt32(bytes, 0);
        }

        public static UInt32 ReadByteValFromProcess(uint processId, UInt64 addr)
        {
            byte[] bytes = ReadByteFromProcess(processId, addr, 1);
            if (bytes == null || bytes.Length != 1) throw (new InvalidOperationException());
            return (UInt32)bytes[0];
            //(UInt32)BitConverter.(bytes, 0);
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

        const int PAGE_READWRITE = 0x04;
        const int MEM_COMMIT = 0x00001000;
        const int PROCESSOR_ARCHITECTURE_AMD64 = 9;

        public static List<IntPtr> FindListAddressByValueFromProcess(uint processId, Int32 Value)
        {
            List<IntPtr> ress = new List<IntPtr>();
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;


            IntPtr hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, processId);
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            UInt32 bytesRead = 0;  // number of bytes read with ReadProcessMemory
            while (proc_min_address_l < proc_max_address_l)
            {
                int result = VirtualQueryEx(hProc, proc_min_address, out mem_basic_info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    byte[] buffer = new byte[mem_basic_info.RegionSize.ToInt32()];
                    // read everything in the buffer above                    
                    ReadProcessMemory(hProc, (IntPtr)mem_basic_info.BaseAddress, buffer, (uint)mem_basic_info.RegionSize.ToInt32(), ref bytesRead);
                    for (int i = 0; i < mem_basic_info.RegionSize.ToInt32(); i += 4)
                    {
                        if (Value == BitConverter.ToInt32(buffer, i))
                            ress.Add(IntPtr.Add(mem_basic_info.BaseAddress, i));
                    }
                }
                proc_min_address_l += mem_basic_info.RegionSize.ToInt64();
                proc_min_address = new IntPtr(proc_min_address_l);
            }
            return ress;
            throw (new InvalidOperationException());
        }

        public static List<IntPtr> FindListAddressByValueFromProcessCOD(uint processId, UInt64 Value, long start = 0x0, long end = 0x0)
        {
            List<IntPtr> ress = new List<IntPtr>();
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            long proc_min_address_l = start;
            long proc_max_address_l = end;


            IntPtr hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, processId);
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            UInt32 bytesRead = 0;  // number of bytes read with ReadProcessMemory
            while (proc_min_address_l < proc_max_address_l)
            {
                int result = VirtualQueryEx(hProc, proc_min_address, out mem_basic_info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    byte[] buffer = new byte[mem_basic_info.RegionSize.ToInt32()];
                    // read everything in the buffer above                    
                    ReadProcessMemory(hProc, (IntPtr)mem_basic_info.BaseAddress, buffer, (uint)mem_basic_info.RegionSize.ToInt32(), ref bytesRead);
                    for (int i = 0; i < mem_basic_info.RegionSize.ToInt32(); i += 8)
                    {
                        if (Value == BitConverter.ToUInt64(buffer, i))
                            ress.Add(IntPtr.Add(mem_basic_info.BaseAddress, i));
                    }
                }
                proc_min_address_l += mem_basic_info.RegionSize.ToInt64();
                proc_min_address = new IntPtr(proc_min_address_l);
            }
            return ress;
            throw (new InvalidOperationException());
        }

        public static List<IntPtr> FindListAddressByValueFromProcess(uint processId, UInt64 Value)
        {
            List<IntPtr> ress = new List<IntPtr>();
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;


            IntPtr hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, processId);
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            UInt32 bytesRead = 0;  // number of bytes read with ReadProcessMemory
            while (proc_min_address_l < proc_max_address_l)
            {
                int result = VirtualQueryEx(hProc, proc_min_address, out mem_basic_info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    byte[] buffer = new byte[mem_basic_info.RegionSize.ToInt32()];
                    // read everything in the buffer above                    
                    ReadProcessMemory(hProc, (IntPtr)mem_basic_info.BaseAddress, buffer, (uint)mem_basic_info.RegionSize.ToInt32(), ref bytesRead);
                    for (int i = 0; i < mem_basic_info.RegionSize.ToInt32(); i += 8)
                    {
                        if (Value == BitConverter.ToUInt64(buffer, i))
                            ress.Add(IntPtr.Add(mem_basic_info.BaseAddress, i));
                    }
                }
                proc_min_address_l += mem_basic_info.RegionSize.ToInt64();
                proc_min_address = new IntPtr(proc_min_address_l);
            }
            return ress;
            throw (new InvalidOperationException());
        }

        public static List<IntPtr> FindListAddressByValueFromProcess(uint processId, UInt32 Value)
        {
            List<IntPtr> ress = new List<IntPtr>();
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;


            IntPtr hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, processId);
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            UInt32 bytesRead = 0;  // number of bytes read with ReadProcessMemory
            while (proc_min_address_l < proc_max_address_l)
            {
                int result = VirtualQueryEx(hProc, proc_min_address, out mem_basic_info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    byte[] buffer = new byte[mem_basic_info.RegionSize.ToInt32()];
                    // read everything in the buffer above                    
                    ReadProcessMemory(hProc, (IntPtr)mem_basic_info.BaseAddress, buffer, (uint)mem_basic_info.RegionSize.ToInt32(), ref bytesRead);
                    for (int i = 0; i < mem_basic_info.RegionSize.ToInt32(); i += 8)
                    {
                        if (Value == BitConverter.ToUInt32(buffer, i))
                            ress.Add(IntPtr.Add(mem_basic_info.BaseAddress, i));
                    }
                }
                proc_min_address_l += mem_basic_info.RegionSize.ToInt32();
                proc_min_address = new IntPtr(proc_min_address_l);
            }
            return ress;
            throw (new InvalidOperationException());
        }

        public static IntPtr FindAddressByValueFromProcess(uint processId, UInt64 Value)
        {
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;


            IntPtr hProc = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, processId);
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            UInt32 bytesRead = 0;  // number of bytes read with ReadProcessMemory
            while (proc_min_address_l < proc_max_address_l)
            {
                int result = VirtualQueryEx(hProc, proc_min_address, out mem_basic_info, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    byte[] buffer = new byte[mem_basic_info.RegionSize.ToInt32()];
                    // read everything in the buffer above                    
                    ReadProcessMemory(hProc, (IntPtr)mem_basic_info.BaseAddress, buffer, (uint)mem_basic_info.RegionSize.ToInt32(), ref bytesRead);
                    for (int i = 0; i < mem_basic_info.RegionSize.ToInt32(); i += 8)
                    {
                        if (Value == BitConverter.ToUInt64(buffer, i))
                            return IntPtr.Add(mem_basic_info.BaseAddress, i);
                    }
                }
                proc_min_address_l += mem_basic_info.RegionSize.ToInt64();
                proc_min_address = new IntPtr(proc_min_address_l);
            }
            throw (new InvalidOperationException());
        }
        

        /// <summary>
        /// Определяет использует ли целевой процесс WOW64 (т.е. 32 разрядный процесс в 64 разярядной системе)
        /// Для 32 разрядной системы всегда возвращает true
        /// </summary>
        /// <param name="processId">PID процесса</param>
        /// <returns><c>true</c>c> если использует(т.е. 32 разрядный процесс)</returns>
        public static bool IsWOW64(uint processId)
        {
            SYSTEM_INFO native_sys_info = new SYSTEM_INFO();
            GetNativeSystemInfo(out native_sys_info);

            if (native_sys_info.processorArchitecture == PROCESSOR_ARCHITECTURE_AMD64) // x64 (AMD or Intel)
            {
                IntPtr hProc = OpenProcess(PROCESS_QUERY_INFORMATION, false, processId);
                int lpExitCode = 0;
                bool isWOW64 = true;
                GetExitCodeProcess(hProc, ref lpExitCode);
                if (lpExitCode == 259) //STILL_ACTIVE (259)
                {
                    IsWow64Process(hProc, ref isWOW64);
                    CloseHandle(hProc);
                    return isWOW64;
                }
                CloseHandle(hProc);
            }
            else
            {
                //Running on WIN32
                return true;
            }
            throw (new InvalidOperationException());
        }

        private static Encoding ae = Encoding.GetEncoding(
              "utf-8",
              new EncoderExceptionFallback(),
              new DecoderExceptionFallback());

        private static Encoding utf16e = Encoding.Unicode;


		
		
		public static UInt64 FindAddrObjFromSignatureMask(uint procId, UInt64 address, uint size, byte?[] Signature, bool check4Null = true)
        {
            byte[] buffMemmory = Utils.ReadByteFromProcess(procId, address, size);
            if (buffMemmory == null || buffMemmory.Length == 0 || Signature.Count() == 0 || !Signature[0].HasValue) throw (new InvalidOperationException());
            int byteCount = buffMemmory.Length;
            for (uint i = 0; i < buffMemmory.Length; ++i)
            {
                if (buffMemmory[i] == Signature[0])
                {
                    int offsetAddrObj;
                    uint offsetLastOperand;
                    if (GetDwordOffsetSignatureMask(Signature, buffMemmory, i, out offsetAddrObj, out offsetLastOperand))
                    {
                        return address + offsetLastOperand + (ulong)offsetAddrObj;
                    }
                }
            }
            throw (new InvalidOperationException());
        }

        // .text:0000000180601375 4C 8B 15 84 B8 AD 01                    mov     r10, cs:qword_1820DCC00
        // 4C 8B 15 - опкод команды
        // 84 B8 AD 01 - 0x01adb884 смещение от следующей команды до cs:qword_1820DCC00
        // Ищет совпадение сигнатуры с куском памяти по offsetMemory, берет смещение до адреса объекта, в маске сигнатуры в месте смещения должны быть null элементы, по которым будет искаться 4 байта смещения.
        public static bool GetDwordOffsetSignatureMask(byte?[] Signature, byte[] buffMemory, uint offsetMemory, out int offsetAddrObj, out uint offsetLastOperand, bool Check4Null = true)
        {
            offsetAddrObj = 0;
            offsetLastOperand = 0;
            bool res = false;
            if (Check4Null && Signature.Count(x => x == null) != 4) return false;
            if (buffMemory.Length < (offsetMemory + Signature.Length)) return false;
            for (int i = 0; i < Signature.Length; ++i)
            {
                if (!Signature[i].HasValue)
                {
                    if (offsetAddrObj == 0)
                    {
                        offsetAddrObj = BitConverter.ToInt32(buffMemory, (int)offsetMemory + i);
                        offsetLastOperand = offsetMemory + (uint)i + 4;
                    }
                    continue;
                }
                else if (buffMemory[offsetMemory + i] != Signature[i].Value) return res;
            }
            return true;
        }

        public class UnitItem
        {
            public string type;
            public string name;
            public uint helth;
            public uint total_helth;

            public override string ToString()
            {
                return string.Format("{0}-{1}-{2}-{3}", type, name, total_helth, helth);
            }
        }

        public static void ParseListUnitsOld(uint procId, UInt64 addressObjectListUnit, uint startPosition)
        {
            List<UnitItem> listObject = new List<UnitItem>();
            while (startPosition != 0xFFFFFFFF)
            {

                uint[] offsetArray = new uint[] { startPosition * 2 * 8 };

                UInt64 currObjAddrPointer = addressObjectListUnit + startPosition * 2 * 8;
                try
                {
                    UInt64 currObjAddr = PointerChainWrap.GetPtr(procId, currObjAddrPointer, new uint[] { 0 });
                    listObject.Add(getUnitFromNpcObject(procId, currObjAddr));
                    /*
                    UInt64 npc_Description_dota = PointerChainWrap.GetPtr(procId, currObjAddr + 8, new uint[] { 0x18, 0 });
                    UnitItem unit = new UnitItem();
                    bool endString;
                    uint countBytes;
                    string nameObj = GetUtf8StringByAddress(procId, npc_Description_dota, out endString, out countBytes);
                    unit.name = nameObj;
                    npc_Description_dota = PointerChainWrap.GetPtr(procId, currObjAddr + 8, new uint[] { 0x20, 0 });
                    nameObj = GetUtf8StringByAddress(procId, npc_Description_dota, out endString, out countBytes);
                    unit.type = nameObj;
                    unit.helth = PointerChainWrap.GetWordVal(procId, currObjAddr + 0x1c4, new uint[] { 0 });
                    unit.total_helth = PointerChainWrap.GetWordVal(procId, currObjAddr + 0x1c0, new uint[] { 0 });
                    if (unit.name.Contains("tower")) listObject.Add(unit);*/
                    startPosition = PointerChainWrap.GetWordVal(procId, currObjAddrPointer + 0xA, new uint[] { 0 });
                }
                catch
                {

                }
                //currObjAddr = addressList[startPosition * 2 * 8];
                //startPosition = [currObjAddr + 0xA0]
                //[currObjAddr + 0x1c0] // total 
                //[currObjAddr + 0x1c4] // current Helth
                //npc_dota = [currObjAddr + 8] // Descrption npc_dota
                //[npc_dota + 20] // npc_dota_tower
                //[npc_dota + 18] // dota_goodguys_tower3_top                 
            }
        }

        public static UnitItem getUnitFromNpcObject(uint procId, UInt64 NpcObject)
        {
            UnitItem unit = new UnitItem();
            UInt64 npc_Description_dota = PointerChainWrap.GetPtr(procId, NpcObject + 8, new uint[] { 0x18, 0 });
            bool endString;
            uint countBytes;
            string nameObj = GetUtf8StringByAddress(procId, npc_Description_dota, out endString, out countBytes);
            unit.name = nameObj;
            npc_Description_dota = PointerChainWrap.GetPtr(procId, NpcObject + 8, new uint[] { 0x20, 0 });
            nameObj = GetUtf8StringByAddress(procId, npc_Description_dota, out endString, out countBytes);
            unit.type = nameObj;
            unit.helth = PointerChainWrap.GetWordVal(procId, NpcObject + 0x1c4, new uint[] { 0 });
            unit.total_helth = PointerChainWrap.GetWordVal(procId, NpcObject + 0x1c0, new uint[] { 0 });
            return unit;
        }

        public static List<UnitItem> ParseListUnits(uint procId, UInt64 addressObjectListUnit, uint countElement)
        {
            List<UnitItem> listObject = new List<UnitItem>();
            for (uint indexObj = 0; indexObj < countElement; indexObj++)
            {
                UInt64 currObjAddrPointer = addressObjectListUnit + indexObj * 0x10;
                if (ReadByteValFromProcess(procId, currObjAddrPointer + 8) == 1)
                {
                    continue;
                }

                UInt64 objContainerNPC = PointerChainWrap.GetPtr(procId, currObjAddrPointer, new uint[] { 8 });
                if (objContainerNPC == 0)
                {
                    continue;
                }
                UInt64 ObjNPC = PointerChainWrap.GetPtr(procId, objContainerNPC, new uint[] { 0x40, 0 });
                listObject.Add(getUnitFromNpcObject(procId, ObjNPC));
            }

            return listObject;
        }
        
        public static string GetUnicodeStringByAddress(uint procId, UInt64 address, out bool endOfString, out uint procBytes, char end = '\0', uint size = 0x250)
        {
            endOfString = false;
            procBytes = 0;
            byte[] buffGaolMaker = ReadByteFromProcess(procId, address, size);
            if (buffGaolMaker == null || buffGaolMaker.Length == 0) return "";

            int byteCount = buffGaolMaker.Length;
            for (int i = 0; i < buffGaolMaker.Length; i += 2)
            {
                if (end != 0 && buffGaolMaker[i] == end)
                {
                    byteCount = i;
                    procBytes = (uint)byteCount + 1;
                    break;
                }

                if (buffGaolMaker[i] == 0)
                {
                    endOfString = true;
                    if (i == 0)
                    {
                        procBytes = 1;
                        return "";
                    }
                    else
                    {
                        byteCount = i;
                        procBytes = (uint)byteCount + 1;
                        break;
                    }
                }
            }

            try
            {
                return utf16e.GetString(buffGaolMaker, 0, byteCount);
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }

        public static string GetUnicodeStringByAddress(IntPtr handle, UInt64 address, out bool endOfString, out uint procBytes, char end = '\0', uint size = 0x250)
        {
            endOfString = false;
            procBytes = 0;
            var buffGaolMaker = ReadByteFromProcess(handle, address, size);
            if (buffGaolMaker == null || buffGaolMaker.Length == 0) return "";

            var byteCount = buffGaolMaker.Length;
            for (var i = 0; i < buffGaolMaker.Length; i += 2)
            {
                if (end != 0 && buffGaolMaker[i] == end)
                {
                    byteCount = i;
                    procBytes = (uint)byteCount + 1;
                    break;
                }

                if (buffGaolMaker[i] == 0)
                {
                    endOfString = true;
                    if (i == 0)
                    {
                        procBytes = 1;
                        return "";
                    }

                    byteCount = i;
                    procBytes = (uint)byteCount + 1;
                    break;
                }
            }

            try
            {
                return utf16e.GetString(buffGaolMaker, 0, byteCount);
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }

        public static string GetUtf8StringByAddress(uint procId, UInt64 address, out bool endOfString, out uint procBytes, char end = '\0', uint size = 0x250)
        {
            endOfString = false;
            procBytes = 0;
            byte[] buffGaolMaker = Utils.ReadByteFromProcess(procId, address, size);
            if (buffGaolMaker == null || buffGaolMaker.Length == 0) return "";

            int byteCount = buffGaolMaker.Length;
            for (int i = 0; i < buffGaolMaker.Length; ++i)
            {
                if (end != 0 && buffGaolMaker[i] == end)
                {
                    byteCount = i;
                    procBytes = (uint)byteCount + 1;
                    break;
                }
                if (buffGaolMaker[i] == 0)
                {
                    endOfString = true;
                    if (i == 0)
                    {
                        procBytes = 1;
                        return "";
                    }
                    else
                    {
                        byteCount = i;
                        procBytes = (uint)byteCount + 1;
                        break;
                    }
                }
            }

            try
            {
                return ae.GetString(buffGaolMaker, 0, byteCount);
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
        
        public static string GetUtf8StringByAddress(IntPtr procHandle, UInt64 address, out bool endOfString, out uint procBytes, char end = '\0', uint size = 0x250)
        {
            endOfString = false;
            procBytes = 0;
            var buffGaolMaker = Utils.ReadByteFromProcess(procHandle, address, size);
            if (buffGaolMaker == null || buffGaolMaker.Length == 0) return "";

            var byteCount = buffGaolMaker.Length;
            for (var i = 0; i < buffGaolMaker.Length; ++i)
            {
                if (end != 0 && buffGaolMaker[i] == end)
                {
                    byteCount = i;
                    procBytes = (uint)byteCount + 1;
                    break;
                }

                if (buffGaolMaker[i] == 0)
                {
                    endOfString = true;
                    if (i == 0)
                    {
                        procBytes = 1;
                        return string.Empty;
                    }

                    byteCount = i;
                    procBytes = (uint)byteCount + 1;
                    break;
                }
            }

            try
            {
                return ae.GetString(buffGaolMaker, 0, byteCount);
            }
            catch (System.Exception ex)
            {
                return string.Empty;
            }
        }

        private static Object wdLockObj = new Object();
        private static string sWorkingFolder;
        public static string GetWorkingDirectory()
        {
            if (string.IsNullOrEmpty(sWorkingFolder))
            {
                lock (wdLockObj)
                {
                    if (string.IsNullOrEmpty(sWorkingFolder))
                    {
                        string sTmpWork = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                        Directory.CreateDirectory(sTmpWork);
                        sWorkingFolder = sTmpWork;
                    }
                }
            }

            return sWorkingFolder;
        }

        public static byte[] MD5ByteForFile(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    var bytes = md5.ComputeHash(stream);
                    return bytes;
                }
            }
        }

        public static string MD5ForFile(string path)
        {
            return BitConverter.ToString(MD5ByteForFile(path));
        }

        public static string MD5ForString(string baseUri)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var bytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(baseUri));
                return BitConverter.ToString(bytes);
            }
        }

        public static byte[] DownloadFile(string uri)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uri);
                req.Method = "GET";
                var resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var stream = resp.GetResponseStream();
                    return ReadAllBytesFromStream(stream, 8192);
                }
                else
                {
                    throw new Exception("Получен неудволетворительный http ответ " + resp.StatusCode);
                }
            }
            catch (Exception ee)
            {
                throw new Exception("Ошибка при загрузке файла", ee);
            }
        }

        public static string UnicodeToAscii(string path)
        {
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;

            // Convert the string into a byte[].
            byte[] unicodeBytes = unicode.GetBytes(path);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            string asciiString = new string(asciiChars);

            asciiString = asciiString.Replace('?', '�');

            return asciiString;
        }

        public static string CombineUri(string sBaseUri, string sRelative)
        {
            if (sBaseUri.Last() != '/') sBaseUri += "/";
            Uri baseUri1 = new Uri(sBaseUri);
            Uri FullUri = new Uri(baseUri1, sRelative);
            return FullUri.ToString();
        }

        public static void WriteByteArrayToFile(byte[] array, string path)
        {
            var dirpath = DirPathFromPath(path);
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            File.WriteAllBytes(path, array);
        }

        public static string GetMd5ForSrvFile(string baseUri, string relativePath)
        {
            string md5checkFile = CombineUri(baseUri, Path.ChangeExtension(relativePath, "txt"));
            var md5bytesFile = Utils.DownloadFile(md5checkFile);
            string md5String = Encoding.ASCII.GetString(md5bytesFile, 0, 32);

            byte[] md5Array = new byte[16];
            for (int i = 0; i < 16; ++i)
            {
                md5Array[i] = Convert.ToByte(md5String.Substring(i * 2, 2), 16);
            }
            return BitConverter.ToString(md5Array);
        }

        public static void LoadFileToListWithCache(string baseUri, string relativePath, List<string> PathList, string sName)
        {
            string alldata = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            alldata = Path.Combine(alldata, "fifa-evolution");
            alldata = Path.Combine(alldata, "cache");

            string cacheUrlPath = Utils.MD5ForString(baseUri);

            alldata = Path.Combine(alldata, cacheUrlPath);

            string chekedFile = Path.Combine(alldata, relativePath.Replace('/', '\\'));
            if (File.Exists(chekedFile))
            {
                string sValidMd5 = GetMd5ForSrvFile(baseUri, relativePath);
                string sFileMd5 = Utils.MD5ForFile(chekedFile);

                if (sFileMd5 == sValidMd5)
                {
                    foreach (string b in PathList)
                    {
                        string path = Path.Combine(b, sName);
                        File.Copy(chekedFile, path, true);
                    }
                    return;
                }
            }

            var bytes = Utils.DownloadFile(CombineUri(baseUri, relativePath));

            WriteByteArrayToFile(bytes, chekedFile);
            foreach (string b in PathList)
            {
                string path = Path.Combine(b, sName);
                WriteByteArrayToFile(bytes, path);
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            byte[] buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        public static bool IsTwoSArrayEqual(string[] arr1, string[] arr2)
        {
            if (arr1.Length != arr2.Length) return false;
            foreach (var s1 in arr1)
            {
                if (!arr2.Contains(s1)) return false;
            }
            return true;
        }

        public static void LoadFileToPath(string uri, string path)
        {
            var bytes = Utils.DownloadFile(uri);
            WriteByteArrayToFile(bytes, path);
        }

        readonly static DateTime toIntStart = DateTime.UtcNow;
        public static int ToInt(this DateTime dt)
        {
            return Convert.ToInt32(dt.Subtract(toIntStart).TotalMilliseconds);
        }

        public static int GetNewRating(int rA, int rB, double resA)
        {
            double EA = 1 / (1 + Math.Pow(10, Convert.ToDouble(rA - rB) / 400F));
            return Convert.ToInt32(GetRatingCf(Math.Max(rA, rB)) * (resA - EA));
        }

        public static double GetResult(int score1, int score2)
        {
            if (score1 == score2)
            {
                return 0.5;
            }
            else if (score1 > score2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        static double GetRatingCf(int r)
        {
            if (r > 2500)
            {
                return 10;
            }
            else
            {
                return 15;
            }
        }

        #region Вспомогательные функции

        public static double FolderSize(string folder)
        {
            double folderSize = 0.0f;
            if (!Directory.Exists(folder))
                return folderSize;
            else
            {
                try
                {
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        if (File.Exists(file))
                        {
                            FileInfo finfo = new FileInfo(file);
                            folderSize += finfo.Length;
                        }
                    }

                    foreach (string dir in Directory.GetDirectories(folder))
                        folderSize += FolderSize(dir);
                }
                catch (NotSupportedException e)
                {
                    Trace.WriteLine("Unable to calculate folder size: {0}", e.Message);
                }
            }
            return folderSize;
        }

        static byte[] ReadAllBytesFromStream(Stream stream, int cnt)
        {
            int gzip_buffer_size = Math.Max(1, cnt / 2);
            int offset = 0;
            int totalCount = 0;
            byte[] outArray = new byte[cnt];
            for (; ; )
            {
                if (offset + gzip_buffer_size >= outArray.Length)
                {
                    Array.Resize<byte>(ref outArray, outArray.Length * 2);
                }
                int bytesRead = stream.Read(outArray, offset, gzip_buffer_size);
                if (bytesRead == 0)
                {
                    break;
                }
                offset += bytesRead;
                totalCount += bytesRead;
            }
            Array.Resize<byte>(ref outArray, totalCount);
            return outArray;
        }

        public static string FileNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf('\\') + 1);
        }

        public static string DirPathFromPath(string path)
        {
            return path.Substring(0, path.LastIndexOf('\\') + 1);
        }

        public static string ExtractDirectory(string path)
        {
            return path.Substring(0, path.LastIndexOf('\\'));
        }

        #endregion
    }
}
