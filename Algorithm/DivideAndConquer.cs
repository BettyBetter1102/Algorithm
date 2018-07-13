using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmDemo
{
    public class DivideAndConquer
    {
        /// <summary>
        /// 分
        ///x = x1 * B^m + x2 
        ///y = y1* B^m + y2
        ///xy = (x1 * B^m + x2)(y1 * B^m + y2) =>
        /// a = x1* y1
        /// b = x1* y2 + x2* y1
        /// c = x2* y2
        /// xy = a * B^2m + b * B^m + c
        /// b = (x1 + x2)(y1 + y2) - a - c
        /// 由原来的四次乘法，变成三次乘法和几次加减法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>

        public int[] KaratsubaMultiplication(int[] x, int[] y)
        {
            int lenx = x.Length;
            int leny = y.Length;
            int halfx = lenx / 2;
            int halfy = leny / 2;

            if (lenx != leny)
                EqualLen(ref x, ref y);
            if (lenx == 1 && leny == 1)
                return new int[] { x[0] * y[0] };
            int[] x1 = new int[halfx];
            int[] x2 = new int[lenx - halfx];
            int[] y1 = new int[halfy];
            int[] y2 = new int[leny - halfy];
            x.CopyTo(x1, 0);
            x.CopyTo(x2, halfx);
            y.CopyTo(y1, 0);
            y.CopyTo(y2, halfy);
            int[] a = KaratsubaMultiplication(x1, y1);

            int[] c = KaratsubaMultiplication(x2, y2);

            int[] b = Minus(Minus(KaratsubaMultiplication(Add(x1, x2), Add(y1, y2)), a), c);

            return Add(Add(Pow(a, 2 * halfx), Pow(b, halfx)), c);
        }
        private void EqualLen(ref int[] x, ref int[] y)
        {

        }
        /// <summary>
        ///x = x1 * B^m + x2 
        ///y = y1* B^m + y2
        ///x+y = (x1 + y1)* B^2m+ (x2 + y2) =>
        /// a = x1 + y1
        /// b = x2 + y2
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int[] Add(int[] x, int[] y)
        {
            int lenx = x.Length;
            int leny = y.Length;
            int halfx = lenx / 2;
            int halfy = leny / 2;

            if (lenx != leny)
                EqualLen(ref x, ref y);
            if (lenx == 1 && leny == 1)
                return new int[] { x[0] * y[0] };
            int[] x1 = new int[halfx];
            int[] x2 = new int[lenx - halfx];
            int[] y1 = new int[halfy];
            int[] y2 = new int[leny - halfy];
            x.CopyTo(x1, 0);
            x.CopyTo(x2, halfx);
            y.CopyTo(y1, 0);
            y.CopyTo(y2, halfy);
            int[] a = Add(x1, y1);
            int[] b = Add(x2, y2);

            return Add(Pow(a, halfx * 2), b);

        }

        /// <summary>
        ///x = x1 * B^m + x2 
        ///y = y1* B^m + y2
        ///x-y = (x1 - y1)* B^2m+ (x2 - y2) =>
        /// a = x1 - y1
        /// b = x2 - y2
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int[] Minus(int[] x, int[] y)
        {
            int lenx = x.Length;
            int leny = y.Length;
            int halfx = lenx / 2;
            int halfy = leny / 2;

            if (lenx != leny)
                EqualLen(ref x, ref y);
            if (lenx == 1 && leny == 1)
                return new int[] { x[0] * y[0] };
            int[] x1 = new int[halfx];
            int[] x2 = new int[lenx - halfx];
            int[] y1 = new int[halfy];
            int[] y2 = new int[leny - halfy];
            x.CopyTo(x1, 0);
            x.CopyTo(x2, halfx);
            y.CopyTo(y1, 0);
            y.CopyTo(y2, halfy);
            int[] a = Minus(x1, y1);
            int[] b = Minus(x2, y2);
            return Add(Pow(a, halfx * 2), b);
        }
        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="deg">10的几次方</param>
        /// <returns></returns>
        private int[] Pow(int[] x, int deg)
        {
            int[] newx = new int[x.Length + deg];
            x.CopyTo(x, 0);
            for (int i = x.Length; i < x.Length + deg; i++)
            {
                newx[i] = 0;
            }
            return newx;
        }
    }



   


  
}
