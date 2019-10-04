using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static WarTWatcher.Watcher;

namespace WarTWatcher
{
    public partial class Settings : Form
    {
        public Watcher Watcher;

		public Settings()
        {
            InitializeComponent();
			Loaded();
		}

		public void Loaded()
		{
			//var sql_team_listP1 = DBconnect.DBWrap.Procedure("GetTournamentTeams");
			//team_listP1 = new List<Team>();
			//foreach (DataRow row in sql_team_listP1.Rows)
			//{
			//	Team new_team = new Team() { id = (int)row.Field<Int32>("id"), TeamName = row.Field<string>("name") };
			//	team_listP1.Add(new_team);
			//}
			//Player1ListBox.DataSource = team_listP1;
			//Player1ListBox.DisplayMember = "TeamName";
			//Player1ListBox.SelectedItem = team_listP1.Where(x => x.id == p1Id).First();
			//team_listP2 = new List<Team>();
			//foreach (DataRow row in sql_team_listP2.Rows)
			//{
			//	Team new_team = new Team() { id = (int)row.Field<Int32>("id"), TeamName = row.Field<string>("name") };
			//	team_listP2.Add(new_team);
			//}
			//Player2ListBox.DataSource = team_listP2;
			//Player2ListBox.DisplayMember = "TeamName";
			//Player2ListBox.SelectedItem = team_listP2.Where(x => x.id == p2Id).First();
		}

		List<Team> team_listP1 = new List<Team>();
		List<Team> team_listP2 = new List<Team>();

		public void SaveSettings()
        {
            XmlDocument xdoc = new XmlDocument();
            XmlElement root = xdoc.CreateElement("root");

			XmlElement team1 = xdoc.CreateElement("team1");
			XmlText team1_value = xdoc.CreateTextNode(((Team)Player1ListBox.SelectedItem).id.ToString());
			team1.AppendChild(team1_value);
			root.AppendChild(team1);

			XmlElement team2 = xdoc.CreateElement("team2");
			XmlText team2_value = xdoc.CreateTextNode(((Team)Player2ListBox.SelectedItem).id.ToString());
			team2.AppendChild(team2_value);
			root.AppendChild(team2);

			xdoc.AppendChild(root);

            xdoc.Save("settings.xml");
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
    }
}
