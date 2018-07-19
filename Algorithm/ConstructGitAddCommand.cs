using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDemo
{
    public class ConstructGitAddCommand
    {
        string fileStr = @"new file:   LogMailDemo/App.config
        new file:   LogMailDemo/DataLineChart.cs
        new file:   LogMailDemo/FeedDailyCount.cs
        new file:   LogMailDemo/LineChart.cs
        new file:   LogMailDemo/LogMailDemo.csproj
        new file:   LogMailDemo/MailHelper.cs
        new file:   LogMailDemo/Program.cs
        new file:   LogMailDemo/Properties/AssemblyInfo.cs
        new file:   LogMailDemo/SendOverall.cs
        new file:   LogMailDemo/Utils.cs
        new file:   LogMailDemo/log4net.config
        new file:   LogMailDemo/packages.config
modified:   Algorithm/AlgorithmDemo.csproj
modified:   AlgorithmDemo.sln
new file:   LogMailDemo/packages.config
Algorithm/packages.config";
        public void TestConstructGitAddCommand()
        {
            string[] strs = fileStr.Split(new char[] { '\r', '\n' });
            string result = string.Empty;
           
            foreach (var item in strs)
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                string file = string.Empty;
                int index = item.IndexOf('\t');
                if (index > 0 && index < item.Length)
                    file = item.Substring(index).Trim();
                else
                    file = item.Trim();
                result +=string .Format ("{0}\t", file) ;
            }
            if (!string.IsNullOrWhiteSpace(result))
                result = "git add " + result;

        }
    }
}
