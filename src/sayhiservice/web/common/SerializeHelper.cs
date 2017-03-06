using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
namespace common
{
    /// <summary>
    /// 序列化辅助类
    /// </summary>
    public class SerializeHelper
    {
        private SerializeHelper()
        { }
        /// <summary>
        /// 使用二进制序列化特定对象成字符串
        /// </summary>
        /// <typeparam name="T">对象的类型,必须是标识为 [Serializable]</typeparam>
        /// <param name="targetObject">对象</param>
        /// <returns></returns>
        public static string BinarySerializeObjectToBase64String<T>(T targetObject)
        {
            BinaryFormatter fm = new BinaryFormatter();
            using (Stream sm = new MemoryStream())
            {
                fm.Serialize(sm, targetObject);
                sm.Seek(0, SeekOrigin.Begin);
                using (BinaryReader br = new BinaryReader(sm))
                {
                    byte[] bs = br.ReadBytes((int)sm.Length);
                    return Convert.ToBase64String(bs);
                }
            }
        }
        /// <summary>
        /// 使用二进制反序列化字符串成指定对象
        /// </summary>
        /// <typeparam name="T">对象的类型,必须是标识为 [Serializable]</typeparam>
        /// <param name="sBase64String">要反序列化的对象</param>
        /// <returns></returns>
        public static T BinaryDeserializeBase64StringToObject<T>(string sBase64String)
        {
            T targetObject = default(T);
            if (sBase64String.StrIsNull())
                return targetObject;

            BinaryFormatter fm = new BinaryFormatter();
            byte[] bs = Convert.FromBase64String(sBase64String);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bs, 0, bs.Length);
                ms.Seek(0, SeekOrigin.Begin);
                targetObject = (T)fm.Deserialize(ms);
            }
            return targetObject;
        }


        /// <summary>
        /// 使用二进制序列化特定对象到指定的文件中
        /// </summary>
        /// <typeparam name="T">对象的类型,必须是标识为 [Serializable]</typeparam>
        /// <param name="sBinaryFilePath">二进制文件路径</param>
        /// <param name="targetObject">对象</param>
        /// <returns></returns>
        public static bool BinarySerialize<T>(string sBinaryFilePath, T targetObject)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sBinaryFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(sBinaryFilePath));
            }
            BinaryFormatter fm = new BinaryFormatter();
            using (Stream sm = new MemoryStream())
            {
                fm.Serialize(sm, targetObject);
                sm.Seek(0, SeekOrigin.Begin);
                using (BinaryReader br = new BinaryReader(sm))
                {
                    byte[] bs = br.ReadBytes((int)sm.Length);
                    File.WriteAllBytes(sBinaryFilePath, bs);
                }
            }
            return true;
        }
        /// <summary>
        /// 使用二进制反序列化指定文件成特定对象
        /// </summary>
        /// <typeparam name="T">对象的类型,必须是标识为 [Serializable]</typeparam>
        /// <param name="sBinaryFilePath">二进制文件路径</param>
        /// <returns></returns>
        public static T BinaryDeserialize<T>(string sBinaryFilePath)
        {
            T targetObject = default(T);
            if (File.Exists(sBinaryFilePath))
            {
                //恢复文件
                BinaryFormatter fm = new BinaryFormatter();
                //using (Stream ms = File.Open(sBinaryFilePath, FileMode.Open))
                //{
                //    targetObject = (T)fm.Deserialize(ms);
                //}
                byte[] bs = File.ReadAllBytes(sBinaryFilePath);
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(bs, 0, bs.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    targetObject = (T)fm.Deserialize(ms);
                }
            }
            return targetObject;
        }

        /// <summary>
        /// 使用Xml格式序列化对象
        /// </summary>
        /// <typeparam name="T">对象的类型,必须是标识为 [Serializable]</typeparam>
        /// <param name="sXmlFilePath">xml文件路径</param>
        /// <param name="encoding">编码</param>
        /// <param name="targetObject">目标对象</param>
        /// <returns></returns>
        public static bool XmlSerialize<T>(string sXmlFilePath, Encoding encoding, T targetObject)
        {
            if (!Directory.Exists(Path.GetDirectoryName(sXmlFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(sXmlFilePath));
            }
            StringBuilder sb = new StringBuilder();
            XmlSerializer xs = new XmlSerializer(typeof(T));
            XmlWriter xmlWrite = XmlWriter.Create(sb);
            xs.Serialize(xmlWrite, targetObject);
            File.WriteAllText(sXmlFilePath, sb.ToString(), encoding);
            return true;
        }
        /// <summary>
        /// 反序列化指定对象
        /// </summary>
        /// <typeparam name="T">对象的类型,必须是标识为 [Serializable]</typeparam>
        /// <param name="sXmlFilePath">xml文件路径</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string sXmlFilePath, Encoding encoding)
        {
            T targetObject = default(T);
            if (File.Exists(sXmlFilePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                StringReader sr = new StringReader(File.ReadAllText(sXmlFilePath, encoding));
                XmlReader reader = XmlReader.Create(sr);
                targetObject = (T)xs.Deserialize(reader);
            }
            return targetObject;
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            string jsonString = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                jsonString = Encoding.UTF8.GetString(ms.ToArray());

                //替换Json的Date字符串 
                string p = @"\\/Date\((\d+)\+\d+\)\\/";
                MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);
            }
            return jsonString;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string jsonString)
        {
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式 
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                try
                {
                    T obj = (T)ser.ReadObject(ms);
                    return obj;
                }
                catch
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        ///  将Json序列化的时间由/Date(1294499956278+0800)转为字符串
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// 将时间字符串转为Json时间
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

    }
}
