using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace common
{
    public class StringHelper
    {

        /// <summary>
        /// 生成指定位随机数(包含数据大小写字母)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string CreateCheckCode(int n)
        {
            char[] CharArray = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            string sCode = "";
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < n; i++)
            {
                sCode += CharArray[random.Next(CharArray.Length)];
            }
            return sCode;
        }
        /// <summary>
        /// 生成指定位随机数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string CreateCheckCodeWithNum(int n)
        {
            char[] CharArray = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string sCode = "";
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < n; i++)
            {
                sCode += CharArray[random.Next(CharArray.Length)];
            }
            return sCode;
        }

        /// <summary>
        /// 随机串（34字符串随机抽取） (1-9 A-Z) 没有O
        /// </summary>
        /// <param name="ran"></param>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public static string RandomNo(Random ran, int xLen)
        {
            string[] char_array = new string[34];
            char_array[0] = "1";
            char_array[1] = "2";
            char_array[2] = "3";
            char_array[3] = "4";
            char_array[4] = "5";
            char_array[5] = "6";
            char_array[6] = "7";
            char_array[7] = "8";
            char_array[8] = "9";
            char_array[9] = "A";
            char_array[10] = "B";
            char_array[11] = "C";
            char_array[12] = "D";
            char_array[13] = "E";
            char_array[14] = "F";
            char_array[15] = "G";
            char_array[16] = "H";
            char_array[17] = "I";
            char_array[18] = "J";
            char_array[19] = "K";
            char_array[20] = "L";
            char_array[21] = "M";
            char_array[22] = "N";
            char_array[23] = "P";
            char_array[24] = "Q";
            char_array[25] = "R";
            char_array[26] = "S";
            char_array[27] = "T";
            char_array[28] = "W";
            char_array[29] = "U";
            char_array[30] = "V";
            char_array[31] = "X";
            char_array[32] = "Y";
            char_array[33] = "Z";

            string output = "";
            double tmp = 0;
            while (output.Length < xLen)
            {
                tmp = ran.NextDouble();
                output = output + char_array[(int)(tmp * 34)].ToString();
            }
            return output;
        }

        public static String CreateNoncestr()
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < 16; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }
        private static string UrlEncode(string temp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                string t = temp[i].ToString();
                string k = HttpUtility.UrlEncode(t);
                if (t == k)
                {
                    stringBuilder.Append(t);
                }
                else
                {
                    stringBuilder.Append(k.ToUpper());
                }
            }
            return stringBuilder.ToString();
        }


        public static string FormatBizQueryParaMap(Dictionary<string, string> paraMap,
               bool urlencode, bool tolower = true)
        {

            string buff = "";
            try
            {
                var result = from pair in paraMap orderby pair.Key select pair;
                foreach (KeyValuePair<string, string> pair in result)
                {
                    if (pair.Key != "")
                    {

                        string key = pair.Key;
                        string val = pair.Value;
                        if (urlencode)
                        {
                            val = UrlEncode(val);
                        }
                        if (tolower)
                        {
                            buff += key.ToLower() + "=" + val + "&";
                        }
                        else
                        {
                            buff += key + "=" + val + "&";
                        }

                    }
                }

                if (buff.Length == 0 == false)
                {
                    buff = buff.Substring(0, (buff.Length - 1) - (0));
                }
            }
            catch (Exception e)
            {
               // throw new SDKRuntimeException(e.Message);
            }
            return buff;
        }

        public static String Sha1(String s)
        {


            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
					'a', 'b', 'c', 'd', 'e', 'f' };
            try
            {
                //byte[] btInput = System.Text.Encoding.Default.GetBytes(s);
                byte[] btInput = System.Text.Encoding.UTF8.GetBytes(s);//by voidarea

                SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();

                byte[] md = sha.ComputeHash(btInput);
                // 把密文转换成十六进制的字符串形式
                int j = md.Length;
                char[] str = new char[j * 2];
                int k = 0;
                for (int i = 0; i < j; i++)
                {
                    byte byte0 = md[i];
                    str[k++] = hexDigits[(int)(((byte)byte0) >> 4) & 0xf];
                    str[k++] = hexDigits[byte0 & 0xf];
                }
                return new string(str);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return null;
            }
        }
    }
}
