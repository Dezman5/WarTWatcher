using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WarTWatcher.Observer;
using static WarTWatcher.Watcher;
using System.IO;

namespace WarTWatcher
{
	public class MatchInfo
	{

		public int duration { get; set; }
		public int user_id { get; set; }
		public List<MatchLog> Log { get; set; }
		public DateTime dtStart { get; set; }
		public DateTime dtEnd { get; set; }
		public string matchLog { get; set; }
		public int finalTick1 { get; set; }
		public int finalTick2 { get; set; }
		public string guid;


		public string SaveResultIntoDB(bool NeedChange)
		{
			string save_log = "";
			try
			{
				string query = "";
				dtEnd = DateTime.UtcNow;
				duration = Convert.ToInt32((dtEnd - dtStart).TotalSeconds);
				//MatchLog oldRow = new MatchLog();
				var lastRow = Log.OrderBy(x => x.realTime).Last();
				var finalTick1 = lastRow.Ticket1;
				var finalTick2 = lastRow.Ticket2;
                //matchLog = JsonConvert.SerializeObject(Log, Formatting.Indented);
                //File.AppendAllText(@"D:\WT.txt", matchLog);
                //List<MatchLog> Log = JsonConvert.DeserializeObject<List<MatchLog>>(matchLog);
               
              


                if (!NeedChange)
                {
                    query = "START TRANSACTION; INSERT INTO MatchDescription (guid, duration, finalTick1, finalTick2) VALUES ('"
                    + guid + "', " + duration.ToString() + ", " + finalTick1.ToString() + ", " + finalTick2.ToString() 
                    + "); SET @tmp_id_match = @@identity; ";
                    if (Log.Count > 0)
                    {
                        query += "INSERT INTO MatchLog (id_match, realTime, ticket1, ticket2, TimeLeft, Names, Teams, Scores, Kills, Deads, Assists, Grounds, Navals, PlaneNames) VALUES ";
                        foreach (var ms in Log)
                        {

                            query += "(" + "@tmp_id_match" + ", "
                            + ms.realTime.ToString() + ", "
                            + ms.Ticket1.ToString() + ", "
                            + ms.Ticket2.ToString() + ", "
                            + ms.TimeLeft.ToString() + ", \""
                            + ms.Names.ToString() + "\", \""
                            + ms.Teams.ToString() + "\", \""
                            + ms.Scores.ToString() + "\", \""
                            + ms.Kills.ToString() + "\", \""
                            + ms.Deads.ToString() + "\", \""
                            + ms.Assists.ToString() + "\", \""
                            + ms.Grounds.ToString() + "\", \""
                            + ms.Navals.ToString() + "\", \""
                            + ms.PlaneNames.ToString()
                            + "\"), ";
                        }

                        query = query.Substring(0, query.Length - 2) + "; ";
                    }
                    query += "COMMIT;";


                    DBconnect.ExecuteQuery(query);

                }
                else
                {
                    query = "START TRANSACTION; INSERT INTO MatchDescription (guid, duration, finalTick1, finalTick2, changed) VALUES ('"
                   + guid + "', " + duration.ToString() + ", " + finalTick2.ToString() + ", " + finalTick1.ToString() + ", " + 1.ToString()
                   + "); SET @tmp_id_match = @@identity; ";
                    if (Log.Count > 0)
                    {
                        query += "INSERT INTO MatchLog (id_match, realTime, ticket1, ticket2, TimeLeft, Names, Teams, Scores, Kills, Deads, Assists, Grounds, Navals, PlaneNames) VALUES ";
                        foreach (var ms in Log)
                        {
                            var msTeams = "";
                            foreach (var r in ms.Teams.Split(',').ToList())
                            {
                                if (r == "1") msTeams += "2,";
                                if (r == "2") msTeams += "1,";
                                if (r == "0") msTeams += "0,";
                            }


                            query += "(" + "@tmp_id_match" + ", "
                            + ms.realTime.ToString() + ", "
                            + ms.Ticket2.ToString() + ", "
                            + ms.Ticket1.ToString() + ", "
                            + ms.TimeLeft.ToString() + ", \""
                            + ms.Names.ToString() + "\", \""
                            + msTeams + "\", \""
                            + ms.Scores.ToString() + "\", \""
                            + ms.Kills.ToString() + "\", \""
                            + ms.Deads.ToString() + "\", \""
                            + ms.Assists.ToString() + "\", \""
                            + ms.Grounds.ToString() + "\", \""
                            + ms.Navals.ToString() + "\", \""
                            + ms.PlaneNames.ToString()
                            + "\"), ";
                        }

                        query = query.Substring(0, query.Length - 2) + "; ";
                    }
                    query += "COMMIT;";


                    DBconnect.ExecuteQuery(query);
                }
			}
			catch (Exception ex)
			{
				File.AppendAllText(@"D:\WTErrSave.txt", ex.ToString());
				save_log = ex.ToString();
				return save_log;
			}


			if (save_log == "")
				save_log = "Результаты матча сохранены, перепроверьте данные матча!";
			else save_log = "Матч бракованный, матч лог содержит неисправимые ошибки";
			return save_log;
		}





		public MatchInfo()
		{
			guid = Guid.NewGuid().ToString("D");
			Log = new List<MatchLog>();
		}


		
	}



	


	public class MatchLog
    {
        public int realTime { get; set; }
		public int Ticket1 { get; set; }
		public int Ticket2 { get; set; }
		public int TimeLeft { get; set; }

		public string Names { get; set; }
		public string Teams { get; set; }
		public string Scores { get; set; }
		public string Kills { get; set; }
		public string Deads { get; set; }
		public string Assists { get; set; }
		public string Grounds { get; set; }
		public string Navals { get; set; }
		public string PlaneNames { get; set; }

		public List<Player> players { get; set; }
		public MatchLog()
		{
			players = new List<Player>();
		}
	}
}
