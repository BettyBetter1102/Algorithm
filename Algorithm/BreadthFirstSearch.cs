using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDemo
{
    /// <summary>
    /// 宽度优先搜索/广度优先搜索
    /// 基本思想：算法首先搜索和S距离为k的所有顶点，然后再去搜索和S距离为k+l的其他顶点
    /// 广度优先搜索使用队列（queue）来实现
    /// 为了保持搜索的轨迹，宽度优先搜索为每个顶点着色：白色、灰色或黑色。
    /// 算法开始前所有顶点都是白色（代表未被发现的点)，灰色代表已被发现但还没有完全搜索其邻接点的结点，黑色代表已发现并且已经完全搜索。
    /// 思路：
    /// 1、把根节点放到队列的末尾。
    /// 2、每次从队列的头部取出一个元素，查看这个元素所有的下一级元素，把它们放到队列的末尾。并把这个元素记为它下一级元素的前驱。
    /// 3、找到所要找的元素时结束程序。
    /// 4、如果遍历整个树还没有找到，结束程序。 
    /// 伪代码：图G=(V,E)（v是点的集合，E是边的集合，s是源节点）
    /// BFS(G,S)
    ///foreach u∈V[G]-{s}
    ///do
    ///color[u]←White;
    ///d[u]←∞;
    ///π[u]←NIL;
    ///end;//初始化所有节点为未发现节点
    ///color[s]←Gray;//初始化源节点为已发现的点
    ///d[s]←0;
    ///π[s]←NIL;
    ///Q←{s}//初始化队列，仅含源节点，以后Q队列中仅包含灰色结点的集合
    ///while(Q≠φ)
    ///do
    ///u←head[Q];
    ///for each v∈Adj[u]//循环考察u连接的每一个顶点v
    ///do
    ///if(color[v]=White)
    ///then
    ///color[v]←Gray;
    ///d[v]←d[u]+1;
    ///π[v]←u;
    ///Enqueue(Q, v);//放入队列Q的队尾
    ///end;
    ///Dequeue(Q);//u的所有节点都被搜索后，u弹出队列，并置为黑色
    ///color[u]←Black;
    ///end;
    ///end;
    ///end;
    /// more Information:https://baike.baidu.com/item/宽度优先搜索
    /// </summary>
    public class BreadthFirstSearch
    {
        int n;
        int m;
        int[,] drections = new int[4, 2] { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 } };
        int[,] visited;

        #region
        /// 例题2：九度1335：闯迷宫 （BFS） 
        /// 题目描述：
        ///sun所在学校每年都要举行电脑节，今年电脑节有一个新的趣味比赛项目叫做闯迷宫。
        ///sun的室友在帮电脑节设计迷宫，所以室友就请sun帮忙计算下走出迷宫的最少步数。
        ///知道了最少步数就可以辅助控制比赛难度以及去掉一些没有路径到达终点的map。
        ///比赛规则是：从原点（0,0）开始走到终点（n-1,n-1），只能上下左右4个方向走，只能在给定的矩阵里走。
        ///输入：
        ///输入有多组数据。
        ///每组数据输入n（0<n<=100），然后输入n* n的01矩阵，0代表该格子没有障碍，为1表示有障碍物。
        ///注意：如果输入中的原点和终点为1则这个迷宫是不可达的。
        ///输出：
        ///对每组输入输出该迷宫的最短步数，若不能到达则输出-1。
        ///样例输入：
        ///2
        ///0 1
        ///0 0
        ///5
        ///0 0 0 0 0
        ///1 0 1 0 1
        ///0 0 0 0 0
        ///0 1 1 1 0
        ///1 0 1 0 0
        ///样例输出：
        ///2
        ///8
        int[,] matrix;

        public struct Point
        {
            public int x;//横坐标
            public int y;//纵坐标
            public int s;//状态0还是1
            //public int v;//是否visited
            public int d;//距离
        };

        private bool IsLegal(int x, int y)
        {
            return x >= 0 && x < n && y >= 0 && y < n;
        }

        private void InitMatix()
        {
            n = 5;
            matrix = new int[n, n];

            visited = new int[n, n];
            //矩阵的大小
            string[] mapStr = new string[] { "0 0 0 0 0", "1 0 1 0 1", "0 0 0 0 0", "0 1 1 1 0", "1 0 1 0 0" };
            for (int i = 0; i < n; i++)
            {
                string[] temp = mapStr[i].Split(' ');
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = Convert.ToInt32(temp[j]);

                }
            }

        }

        private int BFS(Point begin, Point end)
        {
            visited[begin.x, begin.y] = 1;
            begin.d = 0;
            Stack<Point> q = new Stack<Point>();
            q.Push(begin);

            while (q.Count > 0)
            {
                Point current = q.Pop();

                for (int i = 0; i < 4; i++)
                {
                    int nx = current.x + drections[i, 0];
                    int ny = current.y + drections[i, 1];
                    if (IsLegal(nx, ny) && visited[nx, ny] == 0 && matrix[nx, ny] == 0)
                    {
                        visited[nx, ny] = 1;
                        Point next = new Point() { x = nx, y = ny, d = current.d + 1, s = matrix[nx, ny] };

                        if (next.x.Equals(end.x) && next.y.Equals(end.y))
                        {
                            return next.d;
                        }
                        q.Push(next);

                    }
                }
            }
            return -1;


        }
        public void TestBFS()
        {

            InitMatix();
            Point begin = new Point() { x = 0, y = 0, d = 0, s = matrix[0, 0] };
            Point end = new Point() { x = n - 1, y = n - 1, d = -1, s = matrix[n - 1, n - 1] };
            if (begin.s == 1 || end.s == 1)
            {
                Console.WriteLine("-1\n");
                return;
            }


            int result = BFS(begin, end);

            Console.WriteLine(result);
            Console.ReadLine();

        }
        #endregion
    }
}
