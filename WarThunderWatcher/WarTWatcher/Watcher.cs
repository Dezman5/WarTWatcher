using SendKeys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WarTWatcher
{
	public partial class Watcher : Form
	{
		public int id_user;
		public int id_user2;
		public string VideoFolder;
		public static MatchInfo MatchInfo { get; set; }
		public static System.Windows.Forms.Timer MainTimer;
		public static object lock_object = new object();
		public Watcher()
		{
			//#if DEBUG
			//			id_user = 9;
			//			id_user2 = 10;
			//			p2Id = 1;
			//			p1Id = 0;
			//#endif
			InitializeComponent();
		}

		private void Watcher_Load(object sender, EventArgs e)
		{
			
			//ReadSettings();		
		}


		List<Team> team_listP1 = new List<Team>();
		List<Team> team_listP2 = new List<Team>();
		public static DataTable sql_team_listP1 = new DataTable();
		public static DataTable sql_team_listP2 = new DataTable();
		public int p1Id { get; set; }
		public int p2Id { get; set; }


		public static int GetPid(string ProcName)
		{

			foreach (Process cur_proc in Process.GetProcesses())
			{
				if (cur_proc.ProcessName == ProcName)
				{
					//windowHandle = cur_proc.MainWindowHandle;
					return (int)cur_proc.Id;
				}
			}

			return 0;
		}

		//private void ReadSettings()
		//{
		//	XmlDocument doc = new XmlDocument();
		//	try
		//	{
		//		doc.Load("settings.xml");
		//	}
		//	catch (Exception ex)
		//	{
		//		VideoFolder = "";
		//		return;
		//	}

		//	XmlElement root = doc.DocumentElement;
		//	foreach (XmlNode cur_node in root)
		//	{
		//		if (cur_node.Name == "team1")
		//		{
		//			p1Id = Convert.ToInt32(cur_node.InnerText);
		//		}
		//		if (cur_node.Name == "team2")
		//		{
		//			p2Id = Convert.ToInt32(cur_node.InnerText);
		//		}
		//	}
		//	Loaded();
		//}

		public void LoadTeams()
		{
			//sql_team_listP1 = DBconnect.DBWrap.Procedure("GetTournamentTeams");
			//sql_team_listP2 = DBconnect.DBWrap.Procedure("GetTournamentTeams");
		}

		public class Team
		{
			public string TeamName { get; set; }
			public string shortName { get; set; }
			public int id { get; set; }

			static public Team FromDescriptionRow(DataRow DescriptionRow)
			{
				return new Team()
				{
					TeamName = DescriptionRow["name"].ToString(),
					id = Convert.ToInt32(DescriptionRow["id"]),
				};
			}
		}
		static IntPtr windowHandle { get; set; }
		private void test_Click(object sender, EventArgs e)
		{
           
            //ValidWork();
			//CheckingWork();


			NormalWork();
		}


	

		public void NormalWork()
		{
			
			Observer.Stop();
			Observer.LmlCur = new List<MatchLog>();

			MatchInfo = new MatchInfo();


			MatchInfo.dtStart = DateTime.UtcNow;
			MatchInfo.user_id = id_user;
			XspliteControl.StartLogo();
			System.Threading.Thread.Sleep(1500);
			XspliteControl.StartStopRecord();
			System.Threading.Thread.Sleep(10000);
			XspliteControl.StartGame();
			System.Threading.Thread.Sleep(1500);
			Observer.Start(MatchInfo.dtStart, MatchInfo.guid, GetPid("aces"));
		}


		public bool ended { get; set; }
		public bool end { get; set; }
		public int tab { get; set; }
		public int oldBall { get; set; }
		public int oldFallW { get; set; }
		public int oldRuns { get; set; }
		public bool twoInnings { get; set; }





		public static void RenameVideoFile()
		{
			try
			{
				//var s = System.IO.File.ReadAllText(Environment.CurrentDirectory + "\\Sett.txt");
				DirectoryInfo dir = new DirectoryInfo(@"D:\LocalRecording - call1x");
				FileInfo[] files_list = dir.GetFiles("*.mp4");
				string log_message;

				if (files_list.Count() == 0)
				{
					log_message = "не найден ни один видео файл";
					//LogBox.Text = LogBox.Text.Insert(0, log_message);
					return;
				}

				if (MatchInfo == null)
				{
					log_message = "нельзя изменить имя видео записи матча, т.к. запись не произовдилась";
					//LogBox.Text = LogBox.Text.Insert(0, log_message);
					return;
				}

				if (MatchInfo.guid == null)
				{
					log_message = "уникальный идентификатор для матча не инициализирован";
					//LogBox.Text = LogBox.Text.Insert(0, log_message);
					return;
				}

				files_list = files_list.OrderBy(x => x.CreationTime).ToArray();
				FileInfo cur_file = files_list[files_list.Count() - 1];
				log_message = "Запись матча переименованна из " + cur_file.Name + " в " + MatchInfo.guid + ".mp4" + Environment.NewLine + "------------------------------------------------------" + Environment.NewLine;

				File.Move(cur_file.FullName, cur_file.FullName.Substring(0, cur_file.FullName.Length - cur_file.Name.Length) + MatchInfo.guid + ".mp4");
				//LogBox.Text = LogBox.Text.Insert(0, log_message);
			}
			catch (Exception ex)
			{
				//LogBox.Text = LogBox.Text.Insert(0, "Ошибка при переименовывании видео файла, переименуйте его вручную" + ex.ToString() + Environment.NewLine);
			}
		}


	
		private void Settings_Click(object sender, EventArgs e)
		{
			////var t1 = Player1ListBox.SelectedItem as Team;
			////var t2 = Player2ListBox.SelectedItem as Team;


			////LastScore ls = new LastScore(t1, t2);
			////ls.ShowDialog();
			////var res  = ls.firstTeam;
			//ReadSettings();
			//Settings sf = new Settings();
			//sf.ShowInTaskbar = false;
			//sf.ShowDialog(this);
			//ReadSettings();

		}

		private void button1_Click_1(object sкer, EventArgs e)
		{
			try
			{
				Observer.Stop();	
				XspliteControl.StartLogo();
				System.Threading.Thread.Sleep(10000);
				XspliteControl.StartStopRecord();
				System.Threading.Thread.Sleep(5000);
				var dt = DateTime.UtcNow;						
				MatchInfo.Log = Observer.LmlCur;
				System.Threading.Thread.Sleep(2000);
				RenameVideoFile();
				System.Threading.Thread.Sleep(5000);


                var winer = MatchInfo.finalTick1 > MatchInfo.finalTick2 ? "Синий" : "Красный";
                DialogResult dialogResult = MessageBox.Show("Победил"+ winer + "изменить победителя?", "Счёт в матче: " + MatchInfo.finalTick1.ToString() + ":" + MatchInfo.finalTick2.ToString(), MessageBoxButtons.YesNo);
                bool needChange = false;
                if (dialogResult == DialogResult.Yes)
                {
                    needChange = true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    needChange = false;
                }
                var result = MatchInfo.SaveResultIntoDB(needChange);
			    System.Threading.Thread.Sleep(2000);

				Observer.endMatch = false;
				MessageBox.Show("Видео успешно сохранёно в базу, можно начинать следующую игру");
				ended = false;
			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.ToString());
			}
		}
		
		private void Watcher_FormClosed(object sender, FormClosedEventArgs e)
		{
			var processes = Process.GetProcesses();
			foreach (var p in processes)
			{
				if (p.ProcessName == "WarTWatcher")
				{
					p.Kill();
				}
			}
		}
	}
}
