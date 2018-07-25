using log4net.Appender;
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
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(assemblyDirPath, "log4net.config")));
            int timeout = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ConnectionTimeout")) ? 90 : int.Parse(ConfigurationManager.AppSettings.Get("ConnectionTimeout"));
            try
            {

                throw new Exception("Hello World!");
            }
            catch (Exception e)
            {
                log.ErrorFormat("exception:{0}", e.Message);
                log.Error(e.StackTrace);
                var appender = log4net.LogManager.GetRepository().GetAppenders().First(P => P.Name.Equals("RollingLogFileAppender")) as RollingFileAppender;
                String filePath = appender.File;
                appender.Close();
                SendOverall so = new SendOverall();
                so.Send(filePath, e.Message, e.StackTrace, "ChunCheng");
            }
        }
       




    }
}
