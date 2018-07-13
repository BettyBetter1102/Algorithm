//大数加法：基本思想是分治法
//假设两个数都是n位的，把它们分为两个部分
//x=x1*10^m+x2
//y=y1*10^m+y2
//则x+y=(x1+x2)*10^m+(x2+y2)
//m=n%2==0?n/2+1:n/2;
//del=n-m
//void Main(string[] args)
//{
//    string x='123456789';
//    string y='123456789';
//    string result=Add(x,y);
//    Console.WriteLine(result);
//    Console.ReadLine();
//}
using System;
using System.Text;
public class StringAdd
{

    public void TestStringAdd()
    {
        string x = "12345679650";
        string y = "23456789";
        string result = Add(x, y);
        string[] ps = new string[] { "4645", "687978", "787987" };
        result = Add(result, ps);
        Console.WriteLine(result);
        Console.ReadLine();
    }

    private string Add(string x, params string[] addend)
    {
        if (addend == null)
            return x;
        string result = x;
        if (addend != null && !string.IsNullOrWhiteSpace(x))
        {
            foreach (var item in addend)
            {

                result = Add(x, item);
                x = result;
            }
        }
        return result;
    }

    private string Add(string x, string y)
    {


        string result = string.Empty;
        int n = EqualLength(ref x, ref y);
        if (n > 3)
        {
            int m = n % 2 != 0 ? n / 2 + 1 : n / 2;
            int del = n - m;//del=m-1或者del=m
            string x1 = x.Substring(0, del);
            string x2 = x.Substring(del);//x2.Length=n-del=m
            string y1 = y.Substring(0, del);
            string y2 = y.Substring(del);//y2.Length=n-del=m
            string a = Add(x1, y1);
            string b = Add(x2, y2);//b.Length=m或者b.Length=m+1
            result = AddConquer(a, m, b);
        }
        else
        {
            result = (Convert.ToInt32(x) + Convert.ToInt32(y)).ToString();
        }

        return result;
    }

    private int EqualLength(ref string x, ref string y)
    {
        if (x.Length != y.Length)
        {
            StringBuilder b = new StringBuilder();
            b.Append('0', Math.Abs(x.Length - y.Length));
            if (x.Length > y.Length)
            {
                y = b.ToString() + y;
            }
            else
            {
                x = b.ToString() + x;
            }
        }
        return x.Length;
    }

    private string AddConquer(string a, int m, string b)
    {
        if (b.Length > m)
        {
            string top = b.Substring(0, 1);
            string bottom = b.Substring(1);
            string result = Add(a, top);

            return result + bottom;

        }
        else
        {
            return a + b;
        }
    }



}



