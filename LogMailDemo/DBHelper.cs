using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace LogMailDemo
{
    public class DBHelper
    {
        private string ConnectionString;
        private string TSQL_GetPartnerIDName;
        private string Proc_Feed_DailyTimeCount = "Proc_Feed_DailyTimeCount";
        public DBHelper()
        {

            ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            TSQL_GetPartnerIDName = ConfigurationManager.AppSettings.Get("GetPartnerIDName");

        }




       
        public List<KeyValuePair<string, string>> GetPartnerIDName()
        {
            if (string.IsNullOrWhiteSpace(TSQL_GetPartnerIDName))
            {
                return new List<KeyValuePair<string, string>>();
            }
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = TSQL_GetPartnerIDName;
                command.CommandType = CommandType.Text;
                List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.HasRows && reader.Read())
                    {

                        if (reader.IsDBNull(reader.GetOrdinal("AppID")) || string.IsNullOrWhiteSpace(reader.GetString(reader.GetOrdinal("AppID"))))
                        {
                            continue;
                        }
                        var kv = new KeyValuePair<string, string>(reader.GetString(reader.GetOrdinal("AppID")), reader.IsDBNull(reader.GetOrdinal("PartnerName")) ? string.Empty : reader.GetString(reader.GetOrdinal("PartnerName")));
                        result.Add(kv);
                    }

                }
                connection.Close();
                return result;
            }
        }

        public List<FeedDailyCount> FeedSelectDailyCount(string appID, DateTime time, int num, int timeout)
        {
            Utils.CheckStringParameterNull(appID, "appid");

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                command.CommandText = Proc_Feed_DailyTimeCount;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@AppID", SqlDbType.NVarChar, 250) { Value = appID, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@Num", SqlDbType.Int) { Value = num, Direction = ParameterDirection.Input });

                command.Parameters.Add(new SqlParameter("@Time", SqlDbType.DateTime) { Value = time, Direction = ParameterDirection.Input });
                command.CommandTimeout = timeout > 0 ? timeout : 90;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<FeedDailyCount> countList = new List<FeedDailyCount>();

                    while (reader.HasRows && reader.Read())
                    {
                        countList.Add(new FeedDailyCount()
                        {
                            AppID = appID,
                            Date = reader.IsDBNull(reader.GetOrdinal("Time")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Time")),
                            Count = reader.IsDBNull(reader.GetOrdinal("Count")) ? 0 : reader.GetInt32(reader.GetOrdinal("Count"))
                        });
                    }

                    return countList;
                }
            }
        }

        public DataLineChart ListFeedDailyCountToDataLineChart(List<FeedDailyCount> lfdc, List<KeyValuePair<string, string>> PartnerIDNames)
        {
            DataLineChart dlc = new DataLineChart();
            List<DataPoint> ldp = new List<DataPoint>();
            dlc.AppID = lfdc[0].AppID;
            dlc.Title = string.IsNullOrWhiteSpace(PartnerIDNames.FirstOrDefault(m => m.Key.Equals(dlc.AppID)).Value) ? dlc.AppID : PartnerIDNames.FirstOrDefault(m => m.Key.Equals(dlc.AppID)).Value;//后续需要替换成partnerName,如果存在partnerName的话
            foreach (var fdc in lfdc)
            {
                ldp.Add(new DataPoint(fdc.Date.ToOADate(), fdc.Count));
            }
            dlc.Data.Add(ldp);
            return dlc;
        }

    }
}
