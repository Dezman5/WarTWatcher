using MemoryReader;
using SendKeys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WarTWatcher
{
    public static class Observer
    {
		
		public static int procId = 0;
        public static System.Threading.Timer MainTimer;
		public static System.Threading.Timer MainTimer2;
		public static bool MainTimer2Work = false;
		public static string oldMess = "";
		public static string curMess = "";
		public static string GuidCur;

		public static string VideoFolder { get; set; }
        public static int Id_user { get; set; }
        public static int pid { get; set; }
		public static DateTime dtStart { get; set; }



		//private static UInt64 Apples1Address = 0x01A64690;
		//public static uint[] Apples1FullChain = { 0x478, 0x548, 0x388, 0x80 };

		private static UInt64 PlayerAddress = 0x1EDF610+0x8088;
		public static uint[] timeLeftFullChain = { 0x3b8 }; //2b

		private static UInt64 TicketsAddress = 0x1EF3420+0x8088;
		public static uint[] tickets1FullChain = { 0x40, 0x40, 0x40, 0x10, 0x18, 0x20 };
		public static uint[] tickets2FullChain = { 0x40, 0x40, 0x10, 0x18, 0x20 };


		//		48 8B 05 ?? ?? ?? ?? 48 63 D2 48 8B 3C для игроков
		//48 89 48 28 48 8B 05 ? ? ? ? 48 3B C3 75 0B 48 8B 40 30 48 89 05 для всего остального

		//		 try
		//                            {
		//                                Dota2PlayersId = Utils.FindAddrObjFromSignatureMask((uint) pid, Convert.ToUInt64((m.BaseAddress + 0x600000).ToInt64()), 0x200000, SignaturePlIdAddr3);
		//                            }
		//                            catch
		//                            {
		//                                try
		//                                {
		//                                    Dota2PlayersId = Utils.FindAddrObjFromSignatureMask((uint) pid, Convert.ToUInt64((m.BaseAddress + 0x600000).ToInt64()), 0x200000, SignaturePlIdAddr4);
		//                                }
		//                                catch (Exception ee)
		//                                {
		//                                    s += ee.ToString();
		//                                }
		//                            }
		static byte?[] SignaturePlayerAddr = new byte?[] { 0x48, 0x8B, 0x05, null, null, null, null, 0x48, 0x63, 0xD2, 0x48, 0x8B, 0x3C };
		static byte?[] SignatureTicketsAddr = new byte?[] { 0x48, 0x89, 0x48, 0x28, 0x48, 0x8B, 0x05, null, null, null, null, 0x48, 0x3B, 0xC3, 0x75, 0x0B, 0x48, 0x8B, 0x40, 0x30, 0x48, 0x89, 0x05 };
		public static void GetSigns()
		{
			string s = "";
			try
			{

				Process p = Process.GetProcessById(pid);
				foreach (ProcessModule m in p.Modules)
				{
					if (m.ModuleName == "aces.exe")
					{

						try
						{
							PlayerAddress = Utils.FindAddrObjFromSignatureMask((uint)pid, Convert.ToUInt64((m.BaseAddress + 0x800000).ToInt64()), 0x200000, SignaturePlayerAddr);
						}
						catch (Exception ee)
						{
							s += ee.ToString();
							//Dota2TableBA = Convert.ToUInt64((D2BA + 0x20d58f8).ToInt64());
						}
						try
						{
							TicketsAddress = Utils.FindAddrObjFromSignatureMask((uint)pid, Convert.ToUInt64((m.BaseAddress + 0x000000).ToInt64()), 0x2000000, SignatureTicketsAddr);
						}
						catch (Exception ee)
						{
							s += ee.ToString();
							//Dota2TableBA = Convert.ToUInt64((D2BA + 0x20d58f8).ToInt64());
						}
					}
				}
			}
			catch
			{ }
		}

		public static void Start(DateTime dtSt, string guid, int idp)
        {
			pid = idp;
			startMatch = false;
			dtStart = dtSt;
			GuidCur = guid;
			LmlCur = new List<MatchLog>();
			GetSigns();
			MainTimer = new System.Threading.Timer(o => TimerEventMemoryRead(), null, 0, 1000);
			
		}

		public static object lock_object = new object();
        public static MatchLog mlCur { get; set; }
        public static List<MatchLog> LmlCur { get; set; }
		public static bool endMatch { get; set; }
		public static bool startMatch { get; set; }
		public static MemoryDataProvider mdp = new MemoryDataProvider("aces");
		public class Player
		{
			public string Name { get; set; }
			public int Team { get; set; }
			public float Score { get; set; }
			public int Kills { get; set; }
			public int Deads { get; set; }
			public int Grounds { get; set; }
			public int Navals { get; set; }
			public int Assists { get; set; }
			public string PlaneName { get; set; }
		}

		private static void TimerEventMemoryRead()
		{
			try
			{
				lock (lock_object)
				{
					mlCur = new MatchLog();
					//var ticket1 = Convert.ToInt32(DataMemory.GetDataMemory("aces", "aces.exe", new List<Address>(){ new Address("short", (long)TicketsAddress, tickets1FullChain) })[0]);
					//var ticket2 = Convert.ToInt32(DataMemory.GetDataMemory("aces", "aces.exe", new List<Address>(){ new Address("short", (long)TicketsAddress, tickets2FullChain) })[0]);
					//var time = Convert.ToInt32(DataMemory.GetDataMemory("aces", "aces.exe", new List<Address>(){ new Address("short", (long)TicketsAddress, timeLeftFullChain) })[0]);
					var ticket1 = (int)Convert.ToUInt32(mdp.GetDataMemoryV2(new Address("short", (long)TicketsAddress, tickets1FullChain), "aces.exe", false, false));
					var ticket2 = (int)Convert.ToUInt32(mdp.GetDataMemoryV2(new Address("short", (long)TicketsAddress, tickets2FullChain), "aces.exe", false, false));
					var time = (int)Convert.ToUInt32(mdp.GetDataMemoryV2(new Address("short", (long)TicketsAddress, timeLeftFullChain), "aces.exe", false, false));
					List<Player> lp = new List<Player>();
					Player p = new Player();
					for (int i = 0; i < 32; i++)
					{
						p = new Player();
				
						p.Name = mdp.GetDataMemoryV2(new Address("string", (long)PlayerAddress, new uint[] { (uint)i * 0x8, 0x10, 0x28 }), "aces.exe", false, false).ToString().Replace("\'","�");
						mlCur.Names += p.Name + ",";
						//p.Name = DataMemory.GetDataMemory("aces", "aces.exe", new List<Address>() {  })[0].ToString();
						p.Score = Convert.ToSingle(mdp.GetDataMemoryV2( new Address("float", (long)PlayerAddress, new uint[] { (uint)i * 0x8, 0x778 }), "aces.exe", false, false));
						mlCur.Scores += (Convert.ToInt32(p.Score)).ToString() + ",";
						p.PlaneName = mdp.GetDataMemoryV2(new Address("string", (long)PlayerAddress, new uint[] { (uint)i * 0x8, 0x6d0, 0xd60, 0x18, 0x0 }), "aces.exe", false, false).ToString().Replace("\'", "�");
						mlCur.PlaneNames += p.PlaneName + ",";
						p.Kills = Convert.ToInt16(mdp.GetDataMemoryV2(new Address("short", (long)PlayerAddress, new uint[] { (uint)i * 0x8, 0x7a0 }), "aces.exe", false, false));
						mlCur.Kills += p.Kills + ",";
						p.Deads = Convert.ToInt16(mdp.GetDataMemoryV2(new Address("short", (long)PlayerAddress, new uint[] { (uint)i * 0x8, 0x7f0 }), "aces.exe", false, false));
						mlCur.Deads += p.Deads + ",";
						p.Assists = Convert.ToInt16(mdp.GetDataMemoryV2(new Address("short", (long)PlayerAddress, new uint[] { (uint)i * 0x8, 0x908 }), "aces.exe", false, false));
						mlCur.Assists += p.Assists + ",";
						p.Grounds = Convert.ToInt16(mdp.GetDataMemoryV2(new Address("short", (long)PlayerAddress, new uint[] { (uint)i * 0x8, 0x818 }), "aces.exe", false, false));
						mlCur.Grounds += p.Grounds + ",";
						p.Navals = Convert.ToInt16(mdp.GetDataMemoryV2(new Address("short", (long)PlayerAddress, new uint[] { (uint)i * 0x8, 0x840 }), "aces.exe", false, false));
						mlCur.Navals += p.Navals + ",";
						var adr = mdp.GetDataMemoryV2(new Address("ulong", (long)PlayerAddress, new uint[] { (uint)i * 0x8 }), "aces.exe", false, false);
						p.Team = DataMemory.ReadByteWarThunder((ulong)adr + 0x208, Convert.ToUInt32(pid));
						mlCur.Teams += p.Team + ",";
						lp.Add(p);
					}
					mlCur.players = lp;
					mlCur.realTime = (int)DateTime.UtcNow.Subtract(dtStart).TotalSeconds;
					mlCur.Ticket1 = ticket1;
					mlCur.Ticket2 = ticket2;
					mlCur.TimeLeft = time;
					LmlCur.Add(mlCur);
				}
			}
			catch (Exception ee)
			{
				var s = ee.ToString();
				//System.IO.File.AppendAllText(@"D:\errorWT_" + GuidCur + ".txt", s + Environment.NewLine);

				//начало карты, флаг ещё не прогрузило
			}
		}

		public static void Stop()
		{
			if (MainTimer != null)
			{
				MainTimer.Change(Timeout.Infinite, Timeout.Infinite);
			}
		}

	}
}
