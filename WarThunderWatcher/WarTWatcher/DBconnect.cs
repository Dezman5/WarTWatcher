using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarTWatcher
{
	public static class DBconnect
	{
		/// <summary>
		///     Переменная для сохраннения вставленного id
		/// </summary>
		static readonly string _id_insert_game = "@tmp_id";

		/// <summary>
		///     Справочник соотношений между Диминым соотношением и тем что заведено в базе данных
		/// </summary>


		//#region dbWrap
		//private static MySqlWrap.MyWrap _dbWrap = null;
		//private static object locker = new object();
		//public static MySqlWrap.MyWrap DBWrap
		//{
		//	get
		//	{
		//		lock (locker)
		//		{

		//			if (_dbWrap == null)
		//			{
		//				_dbWrap = new MySqlWrap.MyWrap
		//				{
		//					ConnectionString = "server=; User Id=;password=;Allow User Variables=True;Persist Security Info=True;database=warthunder;Minimum Pool Size=3;Maximum Pool Size=5",
		//				};
		//			}

		//			return _dbWrap;
		//		}
		//	}
		//}

		//private static MySqlWrap.MyWrap _mDbWrap = null;
		//private static object mlocker = new object();
		//private static MySqlWrap.MyWrap MasterDbWrap
		//{
		//	get
		//	{
		//		lock (mlocker)
		//		{
		//			if (_mDbWrap == null)
		//			{
		//				_mDbWrap = new MySqlWrap.MyWrap
		//				{
		//					ConnectionString = "server=;User Id=;password=;Allow User Variables=True;Persist Security Info=True;database=bukmaker;Minimum Pool Size=2;Maximum Pool Size=20;Connection Lifetime=300",
		//				};
		//			}

		//			return _mDbWrap;
		//		}
		//	}
		//}

		//public static MySqlWrap.MyWrap GetCyber()
		//{
		//	return DBWrap;
		//}

		//public static MySqlWrap.MyWrap GetMaster()
		//{
		//	return MasterDbWrap;
		//}

		//#endregion


		
	

		

		/// <summary>
		/// Выполнение запроса
		/// </summary>
		/// <param name="in_query"> Запрос в БД </param>
		public static void ExecuteQuery(string in_query)
		{
			try
			{
				//DBWrap.ExecuteNonQuery(in_query);
			}
			catch (Exception ee)
			{
				var s = ee.ToString();
			}
		}

	


	}
}
