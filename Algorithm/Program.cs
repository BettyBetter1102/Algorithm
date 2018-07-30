using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region RuleTag转换
                //RuleTagConvert convert = new AlgorithmDemo.RuleTagConvert();
                //convert.ConstructMethod ("");
                #endregion
                #region 构造Command命令
                //ConstructGitAddCommand commnd = new AlgorithmDemo.ConstructGitAddCommand();
                //commnd.TestConstructGitAddCommand();
                #endregion
                #region 测试表参数
                //TableParametersTest test = new TableParametersTest();
                //int partnerID = 108;

                //String APPID = "XIEFfW27dN3C7Vqhfa";
                //List<FeedStatistic> list = new List<FeedStatistic>()
                //{
                //    new FeedStatistic ()
                //    {
                //        partnerId =108,
                //        AppID="XIEFfW27dN3C7Vqhfa",
                //       feedId ="MSRECOMMEND_CONTENT_2276010",
                //      ArticleKey = "108_XIEFfW27dN3C7Vqhfa_-5100120489162261699",
                //    }
                //};

                //DateTime now = DateTime.Now;
                //test.FeedStatisticListUpdateInsert(partnerID, APPID, list, now);
                #endregion
                #region 动态规划
                //DynamicProgramming dp = new DynamicProgramming("ABCBDAB", "BDCABA");
                //dp.PrintLCSTable();
                //dp.PrintLCSSet();

                #endregion

                #region ExcelHelper
                ExcelHelper helper = new AlgorithmDemo.ExcelHelper();
                helper.ZLNameList();
                #endregion
            }
            catch (Exception ex)
            {
                
            }

        }
    }
}
