using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LogMailDemo
{
    public static class Utils
    {
        private static readonly DateTime EpochStartDate = new DateTime(1970, 1, 1);
        private const int MaxTimeDelay = 10 * 60 * 1000;

        public static DateTime BJNow
        {
            get
            {
                return System.TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "China Standard Time");
            }
        }

        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static string GetBlobContainer(string partner, string name)
        {
            return string.Format("{0}-{1}", partner, name);
        }

        public static string TimeStamp
        {
            get
            {
                System.DateTime startTime = System.TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(1970, 1, 1), "China Standard Time");

                long timeStamp = (long)(BJNow - startTime).TotalSeconds;

                return timeStamp.ToString("X");
            }
        }

        public static long UnixTimeStamp
        {
            get
            {
                return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            }
        }

        public static long UnixTimeStamp_Seconds
        {
            get
            {
                return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string GetQueueName(string partner, string type)
        {
            string queue = string.Format("opencrawling-{0}-{1}", partner, type).ToLower();
            StringBuilder sb = new StringBuilder();
            foreach (var c in queue)
            {
                if (c.Equals('-') || Char.IsLetterOrDigit(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }

        public static string GetMd5Hash(string input)
        {
            StringBuilder sBuilder = new StringBuilder();

            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static void CheckStringParameterNull(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void CheckParameterNull(object value, string name)
        {
            if (null == value)
                throw new ArgumentNullException(name);
        }

        public static void CheckIntNonNegative(int value, string name)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name, string.Format("{0} should not be negative", name));
            }
        }

        public static void CheckIntPositve(int value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name, string.Format("{0} should be positive", name));
            }
        }

        public static void CheckStringParameterLength(string value, int length, string name)
        {
            CheckStringParameterNull(value, name);

            if (length != value.Length)
            {
                throw new ArgumentException(string.Format("The length of {0} should be {1}", name, length));
            }
        }

        public static string ExtractValueFromJson(string json, string key)
        {
            if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(key))
                return null;

            var joRaw = JObject.Parse(json);
            foreach (var jo in joRaw)
            {
                if (key.Equals(jo.Key, StringComparison.InvariantCultureIgnoreCase))
                    return jo.Value.ToString();
            }

            return "";
        }

        public static long DEKHash(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            long hash = str.Length;
            for (int i = 0; i < str.Length; i++)
                hash = ((hash << 5) ^ (hash >> 27)) ^ str.ElementAt(i);
            return hash;
        }

        public static string FeedKey(int partnerId, string appId, string feedId)
        {
            return string.Format("{0}_{1}_{2}", partnerId, appId, DEKHash(feedId));
        }

        public static string UserProfileKey(int partnerId, string appId, string userid)
        {
            return string.Format("{0}_{1}_{2}", partnerId, appId, DEKHash(userid));
        }
        public static string ChatUserMsgKey(int partnerId, string appId, string topicId, string roomId, string msgId)
        {
            return string.Format("{0}_{1}_{2}", partnerId, appId, DEKHash(topicId + "_" + roomId + "_" + msgId));
        }
        public static string ChatHostMsgKey(int partnerId, string appId, string roomId, string msgId)
        {
            return string.Format("{0}_{1}_{2}", partnerId, appId, DEKHash(roomId + "_" + msgId));
        }

        public static string ConcatStringList(List<string> strList, string f)
        {
            if (null == strList)
                return "";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strList.Count; i++)
            {
                sb.Append(strList[i]);
                if (i < strList.Count - 1)
                    sb.Append(f);
            }

            return sb.ToString();
        }

        public static string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        public static IDictionary<string, string> ExtractSortedKVFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            Uri uri = new Uri(url);

            NameValueCollection forms = HttpUtility.ParseQueryString(uri.Query);
            IDictionary<string, string> parameters = new Dictionary<string, string>();

            for (int i = 0; i < forms.Count; ++i)
            {
                string key = forms.Keys[i];
                if (key.ToLower().Equals("sign"))
                {
                    continue;
                }

                parameters.Add(key, forms[key]);
            }

            return new SortedDictionary<string, string>(parameters);
        }

        public static string SignatureUrl(string url, string secret)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(secret))
                return "";

            var dict = ExtractSortedKVFromUrl(url);
            if (null == dict)
                return "";

            IEnumerator<KeyValuePair<string, string>> dem = dict.GetEnumerator();

            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    query.Append(key).Append(value);
                }
            }

            byte[] secretKey = Encoding.UTF8.GetBytes(secret);
            HMACSHA1 hmacsha1 = new HMACSHA1(secretKey);
            byte[] bytes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

        public static Dictionary<string, string> GetParametersFromUrl(string url)
        {
            CheckStringParameterNull(url, "url");
            NameValueCollection queries = HttpUtility.ParseQueryString(new Uri(url).Query);
            return GetParametersFromQueries(queries);
        }


        private static Dictionary<string, string> GetParametersFromQueries(NameValueCollection forms)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            for (int i = 0; i < forms.Count; ++i)
            {
                string key = forms.Keys[i];

                parameters.Add(key, forms[key]);
            }

            return parameters;
        }

        public static Dictionary<string, string> GetParametersFromJsonBody(string content)
        {
            CheckStringParameterNull(content, "content");

            var jobj = JObject.Parse(content);
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            foreach (var item in jobj)
            {
                string key = item.Key;

                JToken jToken = item.Value;
                string value = jToken.ToString(Formatting.None);
                if (jToken is JValue)
                {
                    value = null == ((JValue)jToken).Value ? string.Empty : ((JValue)jToken).Value.ToString();
                }

                parameters.Add(key, value);
            }

            return parameters;
        }

        public static string SignJsonBody(string content, string secret)
        {
            CheckStringParameterNull(content, "content");

            return SignatureRequest(GetParametersFromJsonBody(content), secret);
        }

        public static string SignatureRequest(IDictionary<string, string> paramsDict, string secret)
        {
            CheckParameterNull(paramsDict, "paramsDict");

            if (string.IsNullOrEmpty(secret))
                return "";

            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(paramsDict);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !"sign".Equals(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    query.Append(key).Append(value);
                }
            }

            byte[] secretKey = Encoding.UTF8.GetBytes(secret);
            HMACSHA1 hmacsha1 = new HMACSHA1(secretKey);
            byte[] bytes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

        public static bool VerifyTimeStamp(IDictionary<string, string> paramsDict, ref string errorMessage)
        {
            try
            {
                if (!paramsDict.ContainsKey("ts") || string.IsNullOrEmpty(paramsDict["ts"]))
                {
                    errorMessage = "miss ts field in the request!";
                    return false;
                }

                string ts = paramsDict["ts"];
                long lts;
                if (!long.TryParse(ts, out lts))
                {
                    errorMessage = "invalid format of ts, it should be a long integer!";
                    return false;
                }

                long now = DateTime.UtcNow.ToEpoch();
                long nowBJ = DateTime.UtcNow.AddHours(8).ToEpoch();
                if (Math.Abs(now - 1000 * lts) > MaxTimeDelay && Math.Abs(nowBJ - 1000 * lts) > MaxTimeDelay)
                {
                    errorMessage = "invalid ts field!";
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError(string.Format("ERROR, Info:{0}", e.StackTrace));
                errorMessage = e.Message;
                return false;
            }
        }
        public static long ToEpoch(this DateTime date)
        {
            if (date == null)
            {
                return long.MinValue;
            }

            TimeSpan epochTimeSpan = date - EpochStartDate;
            return (long)epochTimeSpan.TotalMilliseconds;
        }

        public static DateTime FromEpoch(this long milliSeconds)
        {
            return EpochStartDate.AddMilliseconds(milliSeconds);
        }
        //public static bool VerifySignature(Dictionary<string, string> paramsDict, SAIPartnerInfo partnerInfo)
        //{
        //    if (VerifySignatureBySecret(paramsDict, partnerInfo))
        //        return true;

        //    var paramsDictEscape = ProcessEscape(paramsDict);
        //    if (VerifySignatureBySecret(paramsDictEscape, partnerInfo))
        //        return true;

        //    var paramsDictEscapeReverse = ProcessEscapeReverse(paramsDict);
        //    if (VerifySignatureBySecret(paramsDictEscapeReverse, partnerInfo))
        //        return true;

        //    // Handle special escape cases for slash "/" -> "\/"
        //    var paramsDictEscapeReverseWithSlash = ProcessEscapeReverseWithSlash(paramsDictEscapeReverse);
        //    if (VerifySignatureBySecret(paramsDictEscapeReverseWithSlash, partnerInfo))
        //        return true;

        //    return false;
        //}

        //private static bool VerifySignatureBySecret(Dictionary<string, string> paramsDict, SAIPartnerInfo partnerInfo)
        //{
        //    string requestSign = paramsDict["sign"];
        //    string sign = SignatureRequest(paramsDict, partnerInfo.XiaoIcePrimarySecret);

        //    if (sign.Equals(requestSign, StringComparison.InvariantCulture))
        //        return true;

        //    sign = SignatureRequest(paramsDict, partnerInfo.XiaoIceSecondarySecret);

        //    if (sign.Equals(requestSign, StringComparison.InvariantCulture))
        //        return true;

        //    return false;
        //}

        private static Dictionary<string, string> ProcessEscape(Dictionary<string, string> paramsDict)
        {
            if (null == paramsDict)
                return null;

            Dictionary<string, string> processed = new Dictionary<string, string>();
            foreach (var key in paramsDict.Keys)
            {
                processed.Add(key, ReplaceEscape(paramsDict[key]));
            }

            return processed;
        }

        private static Dictionary<string, string> ProcessEscapeReverse(Dictionary<string, string> paramsDict)
        {
            if (null == paramsDict)
                return null;

            Dictionary<string, string> processed = new Dictionary<string, string>();
            foreach (var key in paramsDict.Keys)
            {
                processed.Add(key, ReplaceEscapeReverse(paramsDict[key]));
            }

            return processed;
        }

        private static Dictionary<string, string> ProcessEscapeReverseWithSlash(Dictionary<string, string> paramsDict)
        {
            if (null == paramsDict)
                return null;

            Dictionary<string, string> processed = new Dictionary<string, string>();
            foreach (var key in paramsDict.Keys)
            {
                processed.Add(key, ReplaceEscapeReverseWithSlash(paramsDict[key]));
            }

            return processed;
        }



        private static string ReplaceEscape(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Replace(@"\u0085", "\u0085").Replace(@"\u2028", "\u2028").Replace(@"\u2029", "\u2029").Replace(@"\u00A0", "\u00A0").Replace(@"\/", "/");
        }

        private static string ReplaceEscapeReverse(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Replace("\u0085", @"\u0085").Replace("\u2028", @"\u2028").Replace("\u2029", @"\u2029").Replace("\u00A0", @"\u00A0");
        }

        private static string ReplaceEscapeReverseWithSlash(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Replace("/", @"\/");
        }
    }
}
