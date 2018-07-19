using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace LogMailDemo
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {

            string assemblyFilePath = Assembly.GetExecutingAssembly().Location;
            string assemblyDirPath = Path.GetDirectoryName(assemblyFilePath);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(assemblyDirPath, "log4PDV.config")));
            int timeout = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ConnectionTimeout")) ? 90 : int.Parse(ConfigurationManager.AppSettings.Get("ConnectionTimeout"));
            try
            {
                List<List<FeedDailyCount>> PartnerDataCollection = new List<List<FeedDailyCount>>();
                List<KeyValuePair<string, string>> PartnerIDNames = new List<KeyValuePair<string, string>>();
                    //new DBHelper().GetPartnerIDName();
                DateTime recordBeforeTime = new DateTime(Utils.BJNow.Year, Utils.BJNow.Month, Utils.BJNow.Day, Utils.BJNow.Hour, 0, 0);
                List<string> ingnoreList = ConfigurationManager.AppSettings.Get("IgnoreAppIDs").Split(';').ToList();
                foreach (var kv in PartnerIDNames)
                {
                    if (ingnoreList.Contains(kv.Key))
                    {
                        continue;
                    }
                    log.InfoFormat("start at :{0}, {1}", Utils.BJNow.ToString(Utils.DateTimeFormat), string.Format("PartnerID:{0},PartnerName:{1}", kv.Key, kv.Value));
                    try
                    {
                        //PartnerDataCollection.Add(WareHouseDBManager.Instance.FeedSelectDailyCount(kv.Key, recordBeforeTime, 10, timeout));
                    }
                    catch (Exception e)
                    {
                        log.ErrorFormat("exception:{0}", e.Message);
                        log.Error(e.StackTrace);
                        continue;
                    }
                    log.InfoFormat("end at :{0}, {1}", Utils.BJNow.ToString(Utils.DateTimeFormat), string.Format("PartnerID:{0},PartnerName:{1}", kv.Key, kv.Value));
                }

                List<List<FeedDailyCount>> AbnormalPartnerDataCollection = new List<List<FeedDailyCount>>(PartnerDataCollection);
               

                List<DataLineChart> ldlc = new List<DataLineChart>();
                foreach (var pd in PartnerDataCollection)
                {
                    ldlc.Add(ListFeedDailyCountToDataLineChart(pd, PartnerIDNames));
                }
                foreach (var dlc in ldlc)
                {
                    dlc.ImageStream = new LineChart(dlc).SaveChart();
                    dlc.ImageStream.Position = 0;
                }
                SendOverall so = new SendOverall();
                so.Send(ldlc, recordBeforeTime);

                
            }
            catch (Exception e)
            {
                log.ErrorFormat("exception:{0}", e.Message);
                log.Error(e.StackTrace);
            }
        }


        public static DataLineChart ListFeedDailyCountToDataLineChart(List<FeedDailyCount> lfdc, List<KeyValuePair<string, string>> PartnerIDNames)
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
