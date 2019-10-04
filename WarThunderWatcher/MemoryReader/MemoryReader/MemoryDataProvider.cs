using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
namespace MemoryReader
{
    /// <summary>
    /// Рефакторинг GetDataMemory
    /// </summary>
    public class MemoryDataProvider : IDisposable
    {
        string _processName;
        public string ProcessName
        {
            get { return _processName; }
            set
            {
                _processName = value;                
            }
        }

        private IntPtr ProcessHandle { get; set; }
		private static Encoding ae = Encoding.GetEncoding(
			"utf-8",
			new EncoderExceptionFallback(),
			new DecoderExceptionFallback());
		Process _process = null;
        public Process Process
        {
            get { return _process; }
            set
            {
                _process = value;

                if(ProcessHandle != IntPtr.Zero)
                    CloseHandle();

                ProcessHandle = Utils.OpenProcess(Utils.PROCESS_VM_READ | Utils.PROCESS_QUERY_INFORMATION, false, (uint)Process.Id);
            }
        }

        public MemoryDataProvider(string processName)
        {
            ProcessName = processName;
            Process = GetTargetProcess(_processName);
        }

        public MemoryDataProvider(Process process)
        {
            ProcessName = process.ProcessName;
            Process = process;
        }

        public object GetDataMemoryV2(Address searchAddr, string moduleName = "", bool int32 = false, bool needStartAddress = true)
        {

            var baseAddress = GetStartAddress(moduleName, searchAddr.Add);
			if (!needStartAddress)
			{
				baseAddress = (ulong)searchAddr.Add;
			}



			if (searchAddr.Type == "string")
            {
                bool fEndOfString;
                uint procBytes;
                var str = Utils.GetUtf8StringByAddress(ProcessHandle,
                                                       PointerChainWrap.GetPtrV2(ProcessHandle, baseAddress, searchAddr.Chain, int32),
                                                       out fEndOfString,
                                                       out procBytes);
                return str;
            }

            if (searchAddr.Type == "ustring")
            {
                bool fEndOfString;
                uint procBytes;
                var str = Utils.GetUnicodeStringByAddress(ProcessHandle, 
                                                          PointerChainWrap.GetPtrV2(ProcessHandle, baseAddress, searchAddr.Chain, int32), 
                                                          out fEndOfString,
                                                          out procBytes);
                return str;
            }

            if (searchAddr.Type == "uint")
            {
                var ui = PointerChainWrap.GetUintVal(ProcessHandle, baseAddress, searchAddr.Chain, int32);
                return ui;
            }

            if (searchAddr.Type == "float")
            {
                var flo = PointerChainWrap.GetFloatVal(ProcessHandle, baseAddress, searchAddr.Chain, int32);
                return flo;
            }

            if (searchAddr.Type == "ulong")
            {
                var ul = PointerChainWrap.GetUint64Val(ProcessHandle, baseAddress, searchAddr.Chain, int32);
                return ul;
            }

			if (searchAddr.Type == "short")
			{
				var ul = PointerChainWrap.GetShortVal(ProcessHandle, baseAddress, searchAddr.Chain, int32);
				return ul;
			}

			

			return null;
        }

        [Obsolete("Морально и продуктивно устарело.")]
        delegate uint[] D1(uint[] data, uint[] addval);
        [Obsolete("Морально и продуктивно устарело.")]
        public List<object> GetDataMemory(List<Address> listAddress, string moduleName = "", bool int32 = false)
        {
            var objects = new List<object>();
            foreach (var address in listAddress)
            {
                var baseAddress = GetStartAddress(moduleName, address.Add);
                var Pid = (uint)Process.Id;
                // local function
                D1 v = (chain, addval) => { var l = chain.ToList(); l.AddRange(addval); return l.ToArray(); };

                if (address.Type == "string")
                {
                    bool fEndOfString;
                    uint procBytes;
                    var str = Utils.GetUtf8StringByAddress(Pid, PointerChainWrap.GetPtr(Pid, baseAddress, v(address.Chain, new uint[] { }), int32), out fEndOfString, out procBytes);
                    objects.Add(str);
                }
                else if (address.Type == "ustring")
                {
                    bool fEndOfString;
                    uint procBytes;
                    var str = Utils.GetUnicodeStringByAddress(Pid, PointerChainWrap.GetPtr(Pid, baseAddress, v(address.Chain, new uint[] { }), int32), out fEndOfString, out procBytes);
                    objects.Add(str);
                }
                else if (address.Type == "uint")
                {
                    var ui = PointerChainWrap.GetUintVal(Pid, baseAddress, v(address.Chain, new uint[] { }), int32);
                    objects.Add(ui);
                }
                else if (address.Type == "float")
                {
                    var flo = PointerChainWrap.GetFloatVal(Pid, baseAddress, v(address.Chain, new uint[] { }), int32);
                    objects.Add(flo);
                }
                else if (address.Type == "ulong")
                {
                    var ui = PointerChainWrap.GetUint64Val(Pid, baseAddress, v(address.Chain, new uint[] { }), int32);
                    objects.Add(ui);
                }
            }
            return objects;
        }
		public string GetUtf8StringByAddress(UInt64 address, out bool endOfString, out uint procBytes, char end = '\0', uint size = 0x250)
		{
			endOfString = false;
			procBytes = 0;
			var Pid = (uint)Process.Id;
			byte[] buffGaolMaker = Utils.ReadByteFromProcess(Pid, address, size);
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

		public ulong GetStartAdress(string ProcessName, string ModuleName, Int64 Add = 0x0)
		{
			var Pid = (uint)Process.Id;
			var p = Process.GetProcessById((int)Pid).Modules;
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

	//	public ulong GetStartAddress(string moduleName = "", long shift = 0)

        public object GetDataMemoryV2(Address searchAddr)
        {
            if(searchAddr.Type == "float")
            {
                var tmp = GetStartAddress() + (ulong)searchAddr.Add;
                byte[] byteTime = Utils.ReadByteFromProcess((uint)Process.Id, tmp, sizeof(float));
                return BitConverter.ToSingle(byteTime, 0);
            }

            return null;
        }

        public ulong GetStartAddress(string moduleName = "", long shift = 0)
        {
            var ba = IntPtr.Zero;
            if(string.IsNullOrEmpty(moduleName))
                ba = IntPtr.Add(Process.MainModule.BaseAddress, (int)shift);
            else
            {
                foreach (ProcessModule m in Process.Modules)
                {
                    if (m.ModuleName != moduleName)
                        continue;

                    ba = new IntPtr(m.BaseAddress.ToInt64() + shift);
                    break;
                }
            }

            return (ulong)ba.ToInt64();
        }

        protected virtual Process GetTargetProcess(string processName)
        {
            var prc = Process.GetProcessesByName(processName);
            if (prc.Length > 1 || prc.Length == 0)
                return null;

            return prc.First();
        }

        public void Dispose()
        {
            CloseHandle();
            _process?.Dispose();
        }

        public void CloseHandle()
        {
            Utils.CloseHandle(ProcessHandle);
            ProcessHandle = IntPtr.Zero;
        }
    }
}
