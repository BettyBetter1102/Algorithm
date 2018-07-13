using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDemo
{
    /// <summary>
    /// 深度优先搜索(又叫回溯法）
    /// 基本思想：一只向下走，走不通就掉头
    /// 深度优先搜索的实现方式可以采用递归或者栈来实现。
    /// 思路：
    /// （1） 把初始状态放入数组中，设为当前状态；
    /// （2） 扩展当前的状态，产生一个新的状态放入数组中，同时把新产生的状态设为当前状态；
    /// （3） 判断当前状态是否和前面的重复，（即判断当前状态是否合法）如果重复则回到上一个状态，产生它的另一状态；
    /// （4） 判断当前状态是否为目标状态，如果是目标，则找到一个解答，结束算法。
    /// （5） 如果数组为空，说明无解。
    /// 伪代码：
    /// dfs(状态)   
    /// if 状态 是 目标状态
    /// then
    /// dosomething
    /// else
    /// for 每个新状态
    /// if 新状态合法
    /// »dfs(新状态)
    /// 主程序：
    /// dfs(初始状态)
    ///more information： https://baike.baidu.com/item/深度优先搜索
    /// 例题：http://acm.hdu.edu.cn/showproblem.php?pid=1242
    /// 大意就是天使被关到一个N*M矩阵的监狱了，矩阵里每个格可能是墙（#），可能是路（.），也可能是警卫（x）
    /// 墙是过不去的，路是能过去的，有警卫的必须杀了警卫才能过，过一个路花费时间1，杀一个警卫花费时间1.
    /// a代表天使，r代表天使的朋友，a和r都在监狱里，r的个数>=1.只能向上下左右四个方向走
    /// 问题：
    /// 已知N*M的监狱矩阵，求天使的朋友找到天使需要的最短时间。
    /// 如果不存在，输出Poor ANGEL has to stay in the prison all his life.
    /// 1.天使可能有多个朋友，所以不能从朋友的位置开始着天使，只能是从天使找离他最近的朋友
    /// 2.题目要求的是找到一个用时最少的朋友，而不是步数最少
    /// </summary>
    public class DepthFirstSearch
    {
        int n;
        int m;
        char[,] map;
        int[,] drections;
        int[,] visited;
        //int sum = 1;
        int time;
        int miniTime;

        /// <summary>
        /// 回溯法/深度优先搜索
        /// </summary>
        /// <param name="x">当前位置的横坐标</param>
        /// <param name="y">当前位置的横坐标</param>
        /// <returns></returns>
        private void DFS(int x, int y)
        {

            //如果是目标状态，记录时间
            if (map[x, y] == 'r')
            {
                if (miniTime == 0)
                    miniTime = time;
                else if (miniTime > time)
                {
                    miniTime = time;
                }


            }
            else if (map[x, y] != '#')
            {
                int newx = 0, newy = 0;
                //向四个方向走,找到能走的路
                for (int i = 0; i < 4; i++)
                {
                    newx = x + drections[i, 0];
                    newy = y + drections[i, 1];
                    //判断新状态是否合法

                    if (newx >= 0 && newx < n && newy >= 0 && newy < m && map[newx, newy] != '#' && visited[newx, newy] == 0)
                    {
                        if (miniTime>0&&time > miniTime)
                            return;

                        //有警卫,杀警卫
                        if (map[newx, newy].Equals('x'))
                            time += 1;
                        time += 1;
                        //sum++;
                        //visted[newx, newy] = sum;
                        visited[newx, newy] = 1;
                        DFS(newx, newy);
                        //回溯（一条路走到头了，回溯到上一节点，找其他的的路）
                        //sum--;
                        visited[newx, newy] = 0;
                        time -= 1;
                        if (map[newx, newy].Equals('x'))
                            time -= 1;
                    }
                }
            }
        }

        public void TestDFS()
        {
            //矩阵的大小
            string[] mapStr = new string[] { "#.#####.", "#.a#..r.", "#..#x...", "..#..#.#", "#...##..", ".#......", "........" };


            n = mapStr.Length;
            m = mapStr[0].ToArray().Count();
            map = new char[n, m];
            //天使的坐标
            int x = -1;
            int y = -1;
            for (int i = 0; i < n; i++)
            {
                if (mapStr[i].Contains('a'))
                {
                    x = i;
                    y = mapStr[i].IndexOf('a');
                }
                char[] temp = mapStr[i].ToArray();
                for (int j = 0; j < temp.Length; j++)
                {
                    map[i, j] = temp[j];
                }
            }
            if (x == -1 || y == -1)
                return;

            //已经走过的点
            visited = new int[n, m];
            visited[x, y] = 1;
            //方向
            drections = new int[4, 2] { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 } };


            DFS(x, y);


            if (miniTime == 0)
                Console.WriteLine("Poor ANGEL has to stay in the prison all his life.\n");
            else
                Console.WriteLine(string.Format("{0}", miniTime));

            Console.ReadLine();



        }
    }
}
