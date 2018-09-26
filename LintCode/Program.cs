using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            //string s = "leetcode";
            //int r = FirstUniqChar(s);
            //System.Console.Write(r);
            //System.Console.ReadLine();

            // string s = "au";
            //s = "abcabcbb";
            //s = "bbbbb";
            //s = "pwwkew";
            //s = "bwf";
            //s = "lhdrxershqatgswgusoyupexdobckhzvqemnkfirwklcejkabyyypcvvqzxciyyacmpnsxeqjrsndfogdoevrcqjbnmjmmj";
            //int r = LengthOfLongestSubstring(s);
            //System.Console.Write(r);
            //System.Console.ReadLine();


            //int[] nums1 = new int[] { 1,2 };
            //int[] nums2 = new int[] { 1 ,2,3};
            //double r = FindMedianSortedArrays(nums1, nums2);
            //System.Console.Write(r);
            //System.Console.ReadLine();

            //string s = "babad";
            //string r = LongestPalindrome(s);
            //System.Console.Write(r);
            //System.Console.ReadLine();

            //string s = "ABC";
            //int numRows = 2;
            //System.Console.Write(s);
            //System.Console.Write("\r\n");
            //string r = ZigzagConvert(s, numRows);
            //System.Console.Write("\r\n");
            //System.Console.Write(r);
            //System.Console.ReadLine();

            int[] input = { 3, 2, 4 };
            int target = 6;
            int[] output = TwoSum(input, target);
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

        //https://leetcode.com/problems/longest-substring-without-repeating-characters/description/
        //子序列和子字符串的定义要分清
        public static int LengthOfLongestSubstring(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;
            List<char> array = s.ToArray().ToList();
            bool b = array.TrueForAll(t => t.Equals(s[0]));
            if (b)
                return 1;
            int maxSubStrCount = array.Count > 26 ? 26 : s.Length;
            while (maxSubStrCount >= 2)
            {
                for (int i = 0; i <= s.Length - maxSubStrCount; i++)
                {


                    string temp = s.Substring(i, maxSubStrCount);


                    if (temp.Distinct().Count().Equals(temp.Length))
                    {
                        return maxSubStrCount;
                    }
                }

                maxSubStrCount--;
            }
            return maxSubStrCount;

        }

        //https://leetcode.com/problems/median-of-two-sorted-arrays/description/?from=groupmessage

        public static double FindMedianSortedArrays(int[] nums1, int[] nums2)
        {
            double r = -1;
            int[] tempNums = null;
            int len1 = nums1.Length;
            int len2 = nums2.Length;
            int len = -1;
            if (nums1 == null || len1 == 0)
            {
                tempNums = nums2;
                len = len2;
            }
            else if (nums2 == null || len2 == 0)
            {
                tempNums = nums1;
                len = len1;
            }
            else
            {

                len = len1 + len2;

                if (nums1[len1 - 1] < nums2[0])
                {
                    tempNums = nums1.Union<int>(nums2).ToArray();
                }
                else if (nums2[len2 - 1] < nums1[0])
                {
                    tempNums = nums2.Union<int>(nums1).ToArray();
                }
                else
                {

                    tempNums = new int[len];
                    int len1Index = len1 - 1;
                    int len2Index = len2 - 1;
                    for (int i = len - 1; i >= 0; i--)
                    {

                        if (len1Index < 0 && len2Index >= 0)
                        {
                            tempNums[i] = nums2[len2Index];
                            len2Index--;
                            continue;
                        }
                        if (len2Index < 0 && len1Index >= 0)
                        {
                            tempNums[i] = nums1[len1Index];
                            len1Index--;
                            continue;
                        }
                        if (nums1[len1Index] >= nums2[len2Index])
                        {
                            tempNums[i] = nums1[len1Index];
                            len1Index--;
                        }
                        else
                        {
                            tempNums[i] = nums2[len2Index];
                            len2Index--;
                        }
                    }

                }

            }
            if (tempNums != null)
            {

                if (len % 2 == 0)
                {
                    r = (tempNums[len / 2 - 1] + tempNums[len / 2]) / 2.0;

                }
                else
                {
                    r = tempNums[len / 2];
                }

            }
            return r;
        }

        //https://leetcode.com/problems/longest-palindromic-substring/description/
        public static string LongestPalindrome(string s)
        {


            if (s == null || s.Length < 1) return "";
            int start = 0, end = 0;
            for (int i = 0; i < s.Length; i++)
            {
                int len1 = expandAroundCenter(s, i, i);
                int len2 = expandAroundCenter(s, i, i + 1);
                int len = Math.Max(len1, len2);
                if (len > end - start)
                {
                    start = i - (len - 1) / 2;
                    end = i + len / 2;
                }
            }
            string r = string.Empty;
            if (end + 1 <= s.Length)
                r = s.Substring(start, end - start + 1);
            if (String.IsNullOrWhiteSpace(r))
                r = s.Substring(0, 1);
            return r;
        }


        private static int expandAroundCenter(String s, int left, int right)
        {
            int L = left, R = right;
            while (L >= 0 && R < s.Length && s[L] == s[R])
            {
                L--;
                R++;
            }
            return R - L - 1;
        }

        // https://leetcode.com/problems/zigzag-conversion/description/

        public static string ZigzagConvert(string s, int numRows)
        {
            if (numRows == 1) return s;

            StringBuilder ret = new StringBuilder();
            int n = s.Length;
            int cycleLen = 2 * numRows - 2;

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j + i < n; j += cycleLen)
                {
                    ret.Append(s[j + i]);
                    if (i != 0 && i != numRows - 1 && j + cycleLen - i < n)
                        ret.Append(s[j + cycleLen - i]);
                }
            }
            return ret.ToString();
        }
        //https://leetcode.com/problems/two-sum/
        public static int[] TwoSum(int[] nums, int target)
        {
            if (nums == null)
                return nums;
            List<int> numsList = nums.ToList();
            int[] orderNums = numsList.OrderBy(t => t).ToArray();
            int diff = target;

            for (int i = 0; i < orderNums.Length; i++)
            {
                diff -= orderNums[i];
                for (int j = i + 1; j < orderNums.Length; j++)
                {
                    diff -= orderNums[j];
                    if (diff == 0)
                    {
                        return new int[] { numsList.IndexOf (orderNums[i]), numsList.IndexOf(orderNums[j]) };
                    }
                    else if (diff < 0)
                    {
                        diff += orderNums[j];
                        
                    }
                }
               
            }
            return null;
        }

    }
}


