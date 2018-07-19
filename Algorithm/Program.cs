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
            //DepthFirstSearch dfs = new AlgorithmDemo.DepthFirstSearch();
            //dfs.TestDFS();
            //BreadthFirstSearch bfs = new AlgorithmDemo.BreadthFirstSearch();
            //bfs.TestBFS();
            //StringAdd sta = new StringAdd();
            //sta.TestStringAdd();
            //TwoPointers tp = new TwoPointers();
            //tp.TestTwoSumByHash();
            //tp.TestThreeSum();
            //tp.TestThreeSumClosest();
            //tp.TestFourSum();

            RuleTagConvert c = new RuleTagConvert();
            //c.ConstructMethod("", "","");
            //c.ConstructCatagory("", "");

            #region
            TableParametersTest test = new TableParametersTest();
            int partnerID = 108;

            String APPID = "XIEFfW27dN3C7Vqhfa";
            List<FeedStatistic> list = new List<FeedStatistic>()
            {
                new FeedStatistic ()
                {
                    partnerId =108,
                    AppID="XIEFfW27dN3C7Vqhfa",
                   feedId ="MSRECOMMEND_CONTENT_2276010",
                  ArticleKey = "108_XIEFfW27dN3C7Vqhfa_-5100120489162261699",
                }
            };

            DateTime now = DateTime.Now;
            test.FeedStatisticListUpdateInsert(partnerID, APPID, list, now);
            #endregion

        }
    }
}
