using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LintCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //string s = "A man, a plan, a canal: Panama";
            //string output = ReverseStr(s);
            //System.Console.WriteLine(output);

            //IList<string> r = FizzBuzz(15);
            //foreach (var item in r)
            //{
            //    System.Console.Write(item);
            //    System.Console.Write("\n");
            //}
            //System.Console.ReadLine();

            //string[] ops = new string[] { "5", "-2", "4", "C", "D", "9", "+", "+" };

            //int r = CalPoints(ops);
            //System.Console.Write(r);
            //System.Console.ReadLine();

            string s = "leetcode";
            int r = FirstUniqChar(s);
            System.Console.Write(r);
            System.Console.ReadLine();
        }

        //https://leetcode.com/problems/reverse-string/description/
        public static string ReverseStr(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            char[] arry = input.ToArray();
            if (arry == null || arry.Length <= 0)
                return string.Empty;
            StringBuilder output = new StringBuilder();
            for (int i = arry.Length - 1; i >= 0; i--)
            {
                output.Append(input[i]);
            }
            return output.ToString();

        }
        //https://leetcode.com/problems/fizz-buzz/description/
        public static IList<string> FizzBuzz(int n)
        {
            string[] r = new string[n];
            for (int i = 1; i <= n; i++)
            {
                r[i - 1] = string.Empty;
                if (i % 3 == 0)
                {
                    r[i - 1] = "Fizz";

                }
                if (i % 5 == 0)
                {
                    r[i - 1] += "Buzz";
                }
                if (string.IsNullOrWhiteSpace(r[i - 1]))
                {
                    r[i - 1] = i.ToString();
                }

            }
            return r.ToList();

        }
        //https://leetcode.com/problems/first-unique-character-in-a-string/description/
        public static int FirstUniqChar(string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                Dictionary<char, int> dic = new Dictionary<char, int>();
                for (int i = 0; i < s.Length; i++)
                {
                    if (dic.Count >= 26)
                        return -1;
                    if (dic.ContainsKey(s[i]))
                        dic[s[i]]++;
                    else
                        dic.Add(s[i], 1);

                }
                if (dic.ContainsValue(1))
                    return s.IndexOf(dic.First(t => t.Value == 1).Key);
            }
            return -1;
        }
        //https://leetcode.com/problems/baseball-game/description/
        public static int CalPoints(string[] ops)
        {
            int n = ops.Length;
            int r = 0;
            List<int> rlist = new List<int>();
            for (int i = 0; i < n; i++)
            {
                if (ops[i].Equals("C") && rlist.Count > 0)
                {
                    r -= rlist[rlist.Count - 1];
                    rlist.RemoveAt(rlist.Count - 1);
                }
                else
                {
                    if (ops[i].Equals("+"))
                    {
                        rlist.Add(rlist[rlist.Count - 1] + rlist[rlist.Count - 2]);
                    }
                    else if (ops[i].Equals("D"))
                    {
                        rlist.Add(2 * rlist[rlist.Count - 1]);
                    }
                    else
                    {
                        rlist.Add(Convert.ToInt32(ops[i]));
                    }
                    r += rlist[rlist.Count - 1];
                }
            }
            return r;


        }

    }
}
