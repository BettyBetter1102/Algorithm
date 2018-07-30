
using System;
using System.Collections;
using System.Collections.Generic;

namespace AlgorithmDemo
{
    /// <summary>
    ///这里双指针不是指c++里的二级指针(**p这种）是指一种解题思路，通常用于数组，链表
    ///指针的步长不同或者方向不同（ 一个快指针，一个慢指针；或一个在首一个在尾）
    /// </summary>
    public class TwoPointers
    {
        //
        #region 两数求和问题的两种解法
        //两数求和问题的两种解法
        //Given an array of integers, return indices of the two numbers such that they add up to a specific target.
        //You may assume that each input would have exactly one solution.
        //Example:
        //Given nums = [2, 7, 11, 15], target = 9,
        //Because nums[0] + nums[1] = 2 + 7 = 9,
        //return [0, 1].


        public void TestTwoSumByHash()
        {
            int[] nums = new int[] { 2, 7, 11, 15 };
            int target = 9;
            int[] result = TowSumByHashtable(nums, target);
            Console.WriteLine(string.Format("[{0},{1}]", result[0], result[1]));
            Console.ReadLine();
        }

        public void TestTwoSumByTwoPointers()
        {
            int[] nums = new int[] { 2, 7, 11, 15 };
            int target = 9;
            int low = 0;
            int high = nums.Length;
            int[] result = TwoSumByTwoPointers(nums, target, low, high);
            Console.WriteLine(string.Format("[{0},{1}]", result[0], result[1]));
            Console.ReadLine();
        }

        /// <summary>
        /// 利用哈希表求解，复杂度为O(1)
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private int[] TowSumByHashtable(int[] nums, int target)
        {

            int[] result = new int[2];
            if (nums.Length < 2 || nums == null)
                return result;
            int res;
            Hashtable map = new Hashtable();

            for (int i = 0; i < nums.Length; i++)
            {
                res = target - nums[i];
                if (map.Contains(res))
                {
                    object it = map[res];
                    result[0] = System.Convert.ToInt32(it);
                    result[1] = i;
                    return result;
                }
                else
                {
                    map.Add(nums[i], i);
                }
            }
            return result;
        }

        /// <summary>
        /// 通过排序，（排序令原来O(N*N)的复杂度立即变成了O(N)这个思想很重要）然后用两个指针从前后扫描逼近真值，时间复杂度是O(NlogN)
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private int[] TwoSumByTwoPointers(int[] nums, int target, int low, int high)
        {
            if (nums.Length < 2 || nums == null)
                return null;

            int[] sortedNums = new int[nums.Length];
            nums.CopyTo(sortedNums, 0);
            //Quicksort
            Array.Sort(sortedNums);
            int max = sortedNums[nums.Length - 1];

            //Find the two numbers
            while (low < high)
            {
                while (sortedNums[low] + sortedNums[--high] > target) ;
                if (sortedNums[low] + sortedNums[high] == target)
                    break;
                while (sortedNums[++low] + sortedNums[high] < target) ;
                if (sortedNums[low] + sortedNums[high] == target)
                    break;
            }
            int[] result = new int[2];
            //Finf the indices of the two numbers
            int index = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] == sortedNums[low] || nums[i] == sortedNums[high])
                    result[index++] = i;


            }
            return result;
        }

        #endregion

        #region 三数求和问题
        //Given an array S of n integers, are there elements a, b, c in S such that a + b + c = 0? 
        //Find all unique triplets in the array which gives the sum of zero.
        //Note: Elements in a triplet (a, b, c) must be in non-descending order. (ie, a ≤ b ≤ c)结果必须按升序排列
        //The solution set must not contain duplicate triplets.结果不能出现重复的数组
        //For example, given array S = { -1 0 1 2 - 1 - 4},数组里允许重复的数
        //A solution set is:
        //(-1, 0, 1)
        //(-1, -1, 2)
        //思路：把三数求和转换为上面的两数求和，即先确定一个数，让另外两个数的和newTarget=target-第一个数；
        //三数求和中对结果的要求较多，但是一开始就对数组进行排序，选择3个变量，left，mid，right。
        //在循环的时候，永远保证相对顺序就行了。这样在插入结果的时候，就自然是升序的；
        //这里结果要求的是数组中的数而不是数的位置，所以可以不用Hash表,并且可以直接对原数组进行排序
        public void TestThreeSum()
        {
            int[] nums = new int[] { -1, 0, 1, 2, -1, -4 };
            int target = 0;
            List<int[]> result = ThreeSum(nums, target, 0);
            foreach (var item in result)
            {
                Console.WriteLine(string.Format("[{0},{1},{2}]", item[0], item[1], item[2]));
            }
            Console.ReadLine();
        }

        private List<int[]> ThreeSum(int[] nums, int target, int startIndex)
        {
            List<int[]> result = new List<int[]>();
            int len = nums.Length;
            if (nums == null || len < 3)
                return result;
            //QuickOrder
            Array.Sort(nums);

            int max = nums[len - 1];
            if (3 * nums[startIndex ] > target || 3 * max < target)
                return result;

            //第一层循环，先确定第一个数
            int mid;
            int right ;
            int newTarget;
            int sum;
            //for (int left = startIndex; left < nums.Length - 2 && nums[left] <= 0; left++)（第一个数一定是最小的，如果target为0，那么至少有一个是负数，所以left必须是负的）
            for (int left = startIndex; left < nums.Length - 2; left++)
            {
                mid = left + 1;
                right = nums.Length - 1;
                newTarget = target - nums[left];
                //为了防止出现重复的结果，如果一个left指向的数是之前判断过的，跳过
                if (left > 0 && nums[left] == nums[left - 1])
                    continue;
                #region 三数优化
                //太小
                if (nums[left] + 2 * max < target)
                    continue;
                //太大
                if (3 * nums[left] > target)
                    break;
                else if (3 * nums[left] == target)
                {
                    //  is the boundary
                    if (left +1 < right && nums[left + 2] == nums[left])
                        result.Add(new int[] { nums[left], nums[left], nums[left] });
                    break;

                }
                #endregion
                #region 两数优化
                if (2 * nums[mid] > newTarget || 2 * nums[right] < newTarget)
                    return result;
                if(2*nums [mid]== newTarget)
                {
                    if (mid < right && nums[mid + 1] == nums[mid])
                        result.Add(new int[] { nums[mid], nums[mid], nums[mid] });
                    break;
                }
                #endregion
                //mid和right分别从前后逼近真值
                while (mid < right)
                {

                    sum = nums[mid] + nums[right];
                    if (sum == newTarget)
                    {
                        result.Add(new int[] { nums[left], nums[mid], nums[right] });
                        //跳过right和mid的重复匹配
                        int temMid = nums[mid];
                        int temRight = nums[right];
                        while (++mid < right && nums[mid] == temMid) ;
                        while (mid < --right && nums[right] == temRight) ;
                    }
                    else if (sum < newTarget)
                    { mid++; }

                    else
                    { right--; }
                }
            }
            return result;
        }
        #endregion

        #region 三数求和问题的变种
        //Given an array S of n integers, find three integers in S such that the sum is closest to a given number,
        //target.Return the sum of the three integers.You may assume that each input would have exactly one solution.
        //For example, given array S = { -1 2 1 - 4 }, and target = 1. The sum that is closest to the target is 2. (-1 + 2 + 1 = 2).
        //就是三个数的和不一定等于target而是和target最接近
        //所以hash表的方法就不能用了。而且必须遍历所有的可能（才能找到最接近的那个，当然，因为结果是唯一的所有如果结果与之前的一样的话，可以直接跳过）
        //思路：参考三数求和问题的思路，还是先排序，然后mid和right从两端逼近
        //一个数组保存符合条件的和sumNums，设置一个最小差minRes如过符合条件的和与目标的差res比minRes更小，把和加入sumNums
        //结果就是sumNums的最后一个数

        public void TestThreeSumClosest()
        {
            int[] nums = new int[] { -1, 2, 1, -4 };
            int target = 1;
            int result = ThreeSumClosest(nums, target);
            Console.WriteLine(result);
            Console.ReadLine();
        }

        private int ThreeSumClosest(int[] nums, int target)
        {
            if (nums.Length < 3)
                return 0;

            List<int> sumNums = new List<int>();
            //排序
            Array.Sort(nums);
            int minRes = Int32.MaxValue;
            int mid;
            int right = nums.Length - 1;
            int res = 0;
            for (int left = 0; left < nums.Length - 2; left++)
            {

                mid = left + 1;
                while (mid < right)
                {

                    int tempSum = nums[left] + nums[mid] + nums[right];
                    if (sumNums.Contains(tempSum))
                        continue;
                    res = tempSum - target;
                    if (res > 0)
                        right--;
                    else if (res == 0)
                    {
                        return tempSum;
                    }
                    else
                    {
                        mid++;
                        res = Math.Abs(res);
                    }

                    if (minRes > res)
                    {
                        minRes = res;
                        sumNums.Add(tempSum);
                    }
                }
            }
            return sumNums[sumNums.Count - 1];
        }
        #endregion

        #region 四数组问题
        //Given an array S of n integers, are there elements a, b, c, and d in S such that a + b + c + d = target? Find all unique quadruplets in the array which gives the sum of target.
        //Note: Elements in a quadruplet (a, b, c, d) must be in non-descending order. (ie, a ≤ b ≤ c ≤ d) The solution set must not contain duplicate quadruplets.
        //For example, given array S = { 1 0 - 1 0 - 2 2}, and target = 0.
        //A solution set is: (-1, 0, 0, 1) (-2, -1, 1, 2) (-2, 0, 0, 2)
        //思路：先排序
        //确定一个数，然后文件顺利变成3SUM问题，然后就是完完全全3SUM的解决思路了。
        //优化：排除所有不可能的情况，这个优化同时可以应用到上面几个问题
        // 假设我们考虑4个数分别为A B C D（有序），最大值MAX。
        //A太大，退出：（如果4* A > target） 
        //A太小，跳过：（A+4*MAx<target）
        //确定A后求BCD的3SUM问题
        //B太大，退出：（如果3*B> target） 
        //B太小，跳过：（B+3*MAx<target） ……

        public void TestFourSum()
        {
            int[] nums = new int[] { 1, 0, -1, 0, -2, 2 };
            int target = 0;
            List<int[]> result = FourSum(nums, target, 0);
            foreach (var item in result)
            {
                Console.WriteLine(string.Format("[{0},{1},{2},{3}]", item[0], item[1], item[2], item[3]));
            }
            Console.ReadLine();
        }

        private List<int[]> FourSum(int[] nums, int target, int startIndex)
        {
            List<int[]> result = new List<int[]>();
            int len = nums.Length;
            if (nums == null || len < 4)
                return result;
            //QuickOrder
            Array.Sort(nums);

            int max = nums[len - 1];
            if (4 * nums[0] > target || 4 * max < target)
                return result;
            int threeStartIndex = 0;
            int newTarget = 0;
            for (int a = startIndex; a < nums.Length - 3; a++)
            {
                threeStartIndex = a + 1;
                newTarget = target - nums[a];
                if (a > 0 && nums[a] == nums[a - 1])// avoid duplicate
                    continue;
                if (a + 3 * max < target)// a is too small
                    continue;
                if (4 * nums[a] > target)// a is too big
                    break;
                if (4 * nums[a] == target)//a is the boundary
                {
                    if (a + 3 < len && nums[a + 3] == nums[a])
                        result.Add(new int[] { nums[a], nums[a], nums[a], nums[a] });
                    break;

                }
                //转换为3SUM问题
                List<int[]> threeSumResult = ThreeSum(nums, newTarget, threeStartIndex);
                foreach (var item in threeSumResult)
                {
                    result.Add(new int[] { nums[a], item[0], item[1], item[2] });
                }
            }
            return result;


        }
        #endregion
    }
}
