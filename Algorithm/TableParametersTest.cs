using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDemo
{
    public class FeedStatistic
    {
        [JsonProperty(PropertyName = "feedID")]
        public string feedId;

        public int viewCount;
        public int likeCount;
        public int dislikeCount;
        public int saveCount;
        public int shareCount;
        public int showCount;

        public string AppID;
        public string ArticleKey;
        public int partnerId;

        public void Init(string _appID, int _partnerId)
        {
            partnerId = _partnerId;
            AppID = _appID;
            ArticleKey = string.Format("{0}_{1}_{2}", partnerId, _appID, DEKHash(feedId)); ;
        }
        public static long DEKHash(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            long hash = str.Length;
            for (int i = 0; i < str.Length; i++)
                hash = ((hash << 5) ^ (hash >> 27)) ^ str.ElementAt(i);
            return hash;
        }
    }
    public class TableParametersTest
    {
        private static readonly string Proc_FeedStatistic_UpdateInsert = "Proc_FeedStatistic_UpdateInsert";
        
        static readonly string ConnectionString = "Data Source=MININT-H3DM2D6;Initial Catalog=MVCMusicStore;;Integrated Security=SSPI;Persist Security Info=False";
        public void FeedStatisticListUpdateInsert(int partnerId, string appId, List<FeedStatistic> fsList, DateTime now)
        {
           
            if (null == fsList || fsList.Count < 1)
                return;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = Proc_FeedStatistic_UpdateInsert;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@PartnerID", SqlDbType.Int) { Value = partnerId, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@AppID", SqlDbType.NVarChar, 250) { Value = appId, Direction = ParameterDirection.Input });
                command.Parameters.Add(new SqlParameter("@Now", SqlDbType.DateTime2) { Value = now, Direction = ParameterDirection.Input });

                var table = extractFeedStatisticTable(fsList);
                command.Parameters.Add(new SqlParameter("@FSList", SqlDbType.Structured) { Value = table, Direction = ParameterDirection.Input });

                command.ExecuteNonQuery();
            }
        }

        private DataTable extractFeedStatisticTable(List<FeedStatistic> fsList)
        {
            if (null == fsList || fsList.Count < 1)
                return null;

            DataTable table = new DataTable();
            table.Columns.Add("ArticleKey", typeof(string));
            table.Columns.Add("ViewCount", typeof(int));
            table.Columns.Add("LikeCount", typeof(int));
            table.Columns.Add("DislikeCount", typeof(int));
            table.Columns.Add("SaveCount", typeof(int));
            table.Columns.Add("ShareCount", typeof(int));
            table.Columns.Add("ShowCount", typeof(int));

            foreach (var fs in fsList)
            {
                table.Rows.Add(fs.ArticleKey, fs.viewCount, fs.likeCount, fs.dislikeCount, fs.saveCount, fs.shareCount, fs.showCount);
            }

            return table;
        }
    }
}
