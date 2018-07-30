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
            while (true)
            {
                try
                {

                    var appender = log4net.LogManager.GetRepository().GetAppenders().First(P => P.Name.Equals("RollingLogFileAppender")) as RollingFileAppender;
                    String filePath = appender.File;
                    System.Collections.IEnumerator t = log.Logger.Repository.ConfigurationMessages.GetEnumerator();
                    while (t.MoveNext())
                    {
                        string name = t.Current.ToString();
                        int startIndex = name.IndexOf(':');
                        int endIndex = name.IndexOf(':', startIndex + 1);
                        string tt = name.Substring(startIndex + 1, endIndex - (startIndex + 1));
                        if (tt.Equals("FileAppender"))
                        {
                            startIndex = name.IndexOf('[');
                            endIndex = name.IndexOf(']', startIndex + 1);
                            filePath = name.Substring(startIndex + 1, endIndex - (startIndex + 1));
                            break;
                        }


                    }
                }
                catch (Exception e)
                {
                    log.ErrorFormat("exception:{0}", e.Message);
                    log.Error(e.StackTrace);

                    var appender = log4net.LogManager.GetRepository().GetAppenders().First(P => P.Name.Equals("RollingLogFileAppender")) as RollingFileAppender;
                    String filePath = appender.File;

                    //appender.Close();//关闭后下次循环不能获取文件名

                    // SendOverall so = new SendOverall();
                    // so.Send(filePath, e.Message, e.StackTrace, "ChunCheng");

                }
            }
        }
        private static void TestFileIsOpen(string path)
        {

            //FileStream fs = new FileStream(path, FileMode.Open , FileAccess.Read);

            //BinaryReader br = new BinaryReader(fs);

            //br.Read();

            //Console.WriteLine("文件被打开");
            //1是打开，0是关闭
            int result = FileStatus.FileIsOpen(path);

            Console.WriteLine(result);

            //br.Close();

            //Console.WriteLine("文件被关闭");

            //result = FileStatus.FileIsOpen(path);

            //Console.WriteLine(result);

        }





    }
}
