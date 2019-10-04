using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WarTWatcher.Watcher;

namespace WarTWatcher
{
	public partial class LastScore : Form
	{
		public LastScore()
		{
			InitializeComponent();
			Player1ListBox.Enabled = false;
		}

		public LastScore(Team t1, Team t2)
		{
			InitializeComponent();
			Team1 = t1;
			Team2 = t2;
			Player1ListBox.Items.Add(Team1);
			Player1ListBox.Items.Add(Team2);
			Player1ListBox.DisplayMember = "TeamName";
			Player1ListBox.SelectedItem = Player1ListBox.Items[0];
		}


		public int score { get; set; }
		public Team firstTeam { get; set; }
		public Team Team1 { get; set; }
		public Team Team2 { get; set; }

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				score = Convert.ToInt32(textBox1.Text);
				firstTeam = Player1ListBox.SelectedItem as Team;
				this.Close();
			
			}
			catch
			{
				MessageBox.Show("введите коректное число");
			}
			
		}
	}
}
