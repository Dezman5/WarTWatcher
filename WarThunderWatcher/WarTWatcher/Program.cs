using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarTWatcher
{
	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Watcher());
			//#if DEBUG
			//			Application.EnableVisualStyles();
			//			Application.SetCompatibleTextRenderingDefault(false);
			//			Application.Run(new Watcher());

			//#else
			//						Application.EnableVisualStyles();
			//						Application.SetCompatibleTextRenderingDefault(false);
			//						Application.Run(new Authentication());
			//#endif
		}
	}
}
