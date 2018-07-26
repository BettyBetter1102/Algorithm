using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDemo
{
    #region 介绍
    //动态规划是一种设计的技巧，是解决多阶段决策过程中最优化问题的通用方法
    //基本思想：将待求解的问题分解成数个子问题，先求解子问题的解，然后从子问题中得到问题的解（这部分思想与分治法相似）。
    //与分治法不同的是，这些分解的子问题往往不是互相独立的。若用分治法解决，则有的子问题会重复计算多次。通常会用一个表记录所有已解决的子问题的解，
    //不管该子问题后面会不会用到，只有计算过，就记录下来。
    //动态规划实质上是以空间换时间的技术，起关键在于解决冗余计算。
    //因此能用这个思想解决的问题需要满足以下两个特征
    //1.最优子结构：问题一个最优解中所包含的子问题也是最优的（才能写出最优解的递归方程）
    //2.重叠子问题：用递归算法对问题求解是，每次产生的子问题并不总是新问题，有些子问题会被重复计算多次（才能通过避免重复计算来减少运行时间）
    //动态规划解决最优解问题的一般步骤：
    //1.分析最优解的性质，并刻画其结构特征。
    //2.递归地定义最优值。
    //3.以自底向上的方式或自顶向下的记忆化方法计算出最优值。
    //4.根据计算最优值时得到的信息，构造一个最优解
    #endregion
    public class DynamicProgramming
    {

        private String X;
        private String Y;
        private int[,] table;  // 动态规划表
        private List<String> set;

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public DynamicProgramming(String x, String y)
        {
            this.X = x;
            this.Y = y;
        }

        #region 最长公共子序列（LCS Longest Common Subsequence）问题
        #region 分析
        //最长公共子串（Longest Common Substring）与最长公共子序列（Longest Common Subsequence）的区别：
        //子串要求在原字符串中是连续的，而子序列则只需保持相对顺序，并不要求连续。
        //问题描述：给定两个序列：X[1...m] 和Y[1...n]，求在两个序列中同时出现的最长子序列的长度。
        //假设 X 和 Y 的序列如下：
        //X[1...m] = {A, B, C, B, D, A, B }
        //Y[1...n] = {B, D, C, A, B, A}
        //可以看出，X 和 Y 的最长公共子序列有 “BDAB”、“BCAB”、“BCBA”，即长度为4。
        //1.刻画其结构特征：设C[i,j]=|LCS(X[1...i],Y[1...j])|表示序列X[1...i]和Y[1...j]的最长公共子序列的长，则
        //则 C[m,n] = |LCS(x,y)|就是问题的解
        //推导过程如下：
        //设Xi=[x1, x2,..., xi]表示X序列的前i个字符(1<=i<=m); Yj=[y1, y2,..., yj]表示Y序列的前j个字符
        //   假设Z =[z1, z2,...zk]∈LSC(X, Y)即Z是最长公共子序列中的一个
        //      若Xn = Ym, 则Xn或Ym必定是任意最长公共子序列中的一个字符并且是最后一个字符，因为Z∈LSC(X, Y)，所以zk=Xn=Ym。
        //那么Zk-1=[z1, z2,...zk-1]∈LSC(Xn-1, Ym-1),所以此时|LSC(X, Y)|=|LSC(Xn-1, Ym-1)|+1
        //若Xn≠Ym,则分三种情况
        //Xn∈Z即Zk=Xn,Zk≠Ym,则Z∈LSC(X, Ym-1);
        //Ym∈Z即Zk=Ym,Zk≠Xn,则Z∈LSC(Xn-1, Y);
        //Xn，Ym都不属于Z，则Z=LSC(Xn-1, Ym-1)=LSC(X, Ym-1)=LSC(Xn-1, Y);
        //此时|LSC(X, Y)|=max(|LCS(Xn-1 , Y)|,|LCS(X , Ym-1)|)
        //由于上述当Xm ≠ Yn的情况中，求LCS(Xm-1 , Y) 的长度与LCS(X , Yn-1)的长度，这两个问题不是相互独立的：
        //两者都需要求LCS(Xm-1 , Yn-1)的长度。另外两个序列的LCS中包含了两个序列的前缀的LCS，到这里得出结论：
        //问题具有最优子结构性质，考虑用动态规划法。
        //伪代码：动态规划表记录每次的结果
        //LCS(x, y, i, j)
        //int[,] table = new int[x.Length, y.Length];
        //for(int i=0; i<m+1; ++i)
        //{
        //for(int j = 0; j<n+1; ++j)
        //{
        // 第一行和第一列置0
        //if (i == 0 || j == 0)
        //table[i][j] = 0;
        //  else  if x[i] = y[j]
        //        then table[i][j] ← table[i-1][j-1]+1
        //        else table[i][j] ← max{table[i-1][j],table[i][j-1]}
        //    return table
        #endregion

        /// <summary>
        /// 构造表，并返回X和Y的LCS长度
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private int LCS(int m, int n)
        {
            table = new int[m + 1, n + 1];
            for (int i = 0; i < m ; i++)
            {
                for (int j = 0; j < n ; j++)
                {
                    if (X[i] == Y[j])
                    {
                        table[i+1, j+1] = table[i , j ] + 1;
                    }

                    else
                    {
                        table[i+1, j+1] = Max(table[i+1, j ], table[i, j+1]);
                    }
                }
            }
            return table[m, n];
        }

        /// <summary>
        /// 打印LCS结果表
        /// </summary>
        public void PrintLCSTable()
        {

            int m = X.Length ;
            int n = Y.Length;
            int maxLen = 0;
            if (table == null)
                maxLen = LCS(m, n);
            Console.Write(string.Format("LCS的最大长度是{0}\r\n", maxLen));
            for (int i = 0; i < m + 1; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    Console.Write(string.Format("{0} ", table[i, j]));
                }
                Console.Write("\r\n");
            }
            Console.ReadLine();
        }
        #endregion

        #region 输出所有的最长公共子序列
        #region 分析
        //输出一个最长公共子序列并不难（网上很多相关代码），难点在于输出所有的最长公共子序列，因为 LCS 通常不唯一。总之，我们需要在动态规划表上进行回溯 —— 从table[m][n]，即右下角的格子，开始进行判断：
        //如果格子table[i][j] 对应的X[i - 1] == Y[j - 1]，则把这个字符放入 LCS 中，并跳入table[i - 1][j - 1] 中继续进行判断；
        //如果格子table[i][j] 对应的 X[i - 1] ≠ Y[j - 1]，则比较table[i - 1][j] 和table[i][j-1]的值，跳入值较大的格子继续进行判断；
        //直到 i 或 j 小于等于零为止，倒序输出 LCS 。
        //如果出现table[i - 1][j] 等于table[i][j-1]的情况，说明最长公共子序列有多个，故两边都要进行回溯（这里用到递归）。
        #endregion

        /// <summary>
        /// 求两个数中较大的数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        /// <summary>
        /// 字符串逆序
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string Reverse(string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            char[] array = str.ToArray();
            for (int i = array.Length - 1; i >= 0; i--)
            {
                sb.Append(array[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 回溯，求出所有的最长公共子序列，并放入set中
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="lcs_str"></param>
        private void TraceBack(int i, int j, string lcs_str)
        {
            while (i > 0 && j > 0)
            {
                if (X[i-1] == Y[j-1])
                {
                    lcs_str += X[i-1];
                    --i;
                    --j;
                }

                else if (table[i-1, j] > table[i, j-1])
                {
                    --i;
                }

                else if (table[i - 1, j] < table[i, j - 1])
                {
                    --j;
                }

                else
                {
                    TraceBack(i - 1, j, lcs_str);
                    TraceBack(i, j - 1, lcs_str);
                    return;
                }

            }
            set.Add(Reverse(lcs_str));
        }

        public void PrintLCSSet()
        {
            if (set == null)
            {
                set = new List<string>();
                String str = "";
                int m = X.Length;
                int n = Y.Length;
                TraceBack(m, n, str);
            }


            for (int i = 0; i < set.Count; i++)
            {
                Console.WriteLine(set[i] + "\r\n");
            }
            Console.ReadLine();
        }

        #endregion
    }

}
