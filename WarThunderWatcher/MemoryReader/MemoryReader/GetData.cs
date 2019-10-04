using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;

namespace MemoryReader
{
    public static class DataMemory
    {
        public static uint Pid { get; set; }
        delegate uint[] D1(uint[] data, uint[] addval);


		public static int ReadByteWarThunder(UInt64 BaseAddress, uint idp)
		{
			var res = Utils.ReadByteFromProcess(idp, BaseAddress, 1);
			var ret = BitConverter.ToInt32( new byte[] {  res[0],0, 0, 0 }, 0);
			return ret;
		}

        public static List<object> GetDataMemory(string ProcessName, string ModuleName, List<Address> ListAddress, bool int32 = false)
        {
            List<object> objects = new List<object>();
            
            foreach (var address in ListAddress)
            {
                var BaseAddress = GetStartAdress(ProcessName, ModuleName, address.Add);
                D1 v = (chain, addval) => { var l = chain.ToList(); l.AddRange(addval); return l.ToArray(); };

                if (address.Type == "string")
                {
                    bool fEndOfString;
                    uint procBytes;
                    var str = Utils.GetUtf8StringByAddress(Pid, PointerChainWrap.GetPtr(Pid, BaseAddress, v(address.Chain, new uint[] { }), int32), out fEndOfString, out procBytes);
                    objects.Add(str);
                }
				else if (address.Type == "ustring")
				{
					bool fEndOfString;
					uint procBytes;
					var str = Utils.GetUnicodeStringByAddress(Pid, PointerChainWrap.GetPtr(Pid, BaseAddress, v(address.Chain, new uint[] { }), int32), out fEndOfString, out procBytes);
					objects.Add(str);
				}
				else if (address.Type == "ustring")
                {
                    bool fEndOfString;
                    uint procBytes;
                    var str = Utils.GetUnicodeStringByAddress(Pid, PointerChainWrap.GetPtr(Pid, BaseAddress, v(address.Chain, new uint[] { }), int32), out fEndOfString, out procBytes);
                    objects.Add(str);
                }
                else if (address.Type == "uint")
                {
                    var ui = PointerChainWrap.GetUintVal(Pid, BaseAddress, v(address.Chain, new uint[] { }), int32);
                    objects.Add(ui);
                }
                else if (address.Type == "float")
                {
                    var flo = PointerChainWrap.GetFloatVal(Pid, BaseAddress, v(address.Chain, new uint[] { }), int32);
                    objects.Add(flo);
                }
                else if (address.Type == "ulong")
                {
                    var ui = PointerChainWrap.GetUint64Val(Pid, BaseAddress, v(address.Chain, new uint[] { }), int32);
                    objects.Add(ui);
                }
				else if (address.Type == "short")
				{
					var ui = PointerChainWrap.GetShortVal(Pid, BaseAddress, v(address.Chain, new uint[] { }), int32);
					objects.Add(ui);					
				}
				else if (address.Type == "byte")
				{
					var res = Utils.ReadByteFromProcess(Pid, BaseAddress, 1);
					var ui = PointerChainWrap.GetByteVal(Pid, BaseAddress, v(address.Chain, new uint[] { }), int32);
					objects.Add(ui);
				}
				
			}
            return objects;
        }

        static IntPtr windowHandle { get; set; }
        public static int GetPid(string ProcName)
        {
            
            foreach (Process cur_proc in Process.GetProcesses())
            {
                if (cur_proc.ProcessName == ProcName)
                {
                    windowHandle = cur_proc.MainWindowHandle;
                    return (int)cur_proc.Id;
                }
            }

            return 0;
        }

        public static byte[] GetDataMemoryPUBG(IntPtr BaseAddress, uint size, string ProcName)
        {
            
                List<object> objects = new List<object>();
                var Pid = Convert.ToUInt32(GetPidInject(ProcName));
                byte[] buffMemmory = Utils.ReadByteFromProcess(Pid, (UInt64)BaseAddress, size);

                return buffMemmory;
           
        }

        public static ulong GetStartAdress(string ProcessName, string ModuleName, Int64 Add = 0x0)
        {
            var pid = GetPid(ProcessName);
            Pid = (uint)pid;
            var p = Process.GetProcessById(pid).Modules;
            foreach (ProcessModule m in p)
            {
                if (m.ModuleName == ModuleName)
                {
                    var BA = m.BaseAddress;
                    var BaseAddress = Convert.ToUInt64(BA.ToInt64() + Add);
                    return BaseAddress;
                }
            }
            return new ulong();
        }

        public static int GetPidInject(string ProcessName)
        {
            //   Process proc = Process.GetProcessesByName(ProcessName).FirstOrDefault(x => x.VirtualMemorySize64 >= 1000000000 && x.MainWindowHandle != IntPtr.Zero);
            //   return (int)proc.Id;
            var p = Process.GetProcesses();
            foreach (Process cur_proc in p)
            {
                if (cur_proc.ProcessName == ProcessName)
                {
                    return (int)cur_proc.Id;
                }
            }

            return 0;
        }
    }
}
