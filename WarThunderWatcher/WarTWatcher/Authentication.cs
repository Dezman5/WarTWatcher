using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarTWatcher
{
    public partial class Authentication : Form
    {
		public Authentication()
		{
			
			InitializeComponent();
			
		}

		public int userid { get; set; }
		public int teamid { get; set; }

		private void AuthProc()
        {
            //var sql_auth_data = DBconnect.DBWrap.Procedure("UsersAuthentication", Login.Text, Password.Text);

			//if (sql_auth_data.Rows.Count > 0)
			//{
			//		Authentication A = new Authentication();
			//		A.userid = sql_auth_data.Rows[0].Field<Int32>("id");
			//		A.teamid = sql_auth_data.Rows[0].Field<Int32>("idTeam");
			//		A.Show();
			//		this.Visible = false;			
			//}
			//else
			//{
			//	MessageBox.Show("Не верный логин или пароль!", "Ошибка", MessageBoxButtons.OK);
			//}
        }

        private void Auth_Click(object sender, EventArgs e)
        {
            AuthProc();
        }

        private void Password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
            {
                AuthProc();
            }
        }

		private void Authentication_Load(object sender, EventArgs e)
		{

		}
	}
}
