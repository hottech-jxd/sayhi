using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace common
{
    public class LogHelper
    {
        private LogHelper() { }
        private static Object FileObject = new object();

        private static Object FileObject_Success = new object();
        private static Object FileObject_Error = new object();

        #region 写日志
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="content"></param>
        public static void Write(object content)
        {
            string sLogDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logPath = "Log";
            string logPathFull = null;
            if (HttpContext.Current != null)
            {
                logPath = Path.DirectorySeparatorChar + logPath;
                logPathFull = HttpContext.Current.Server.MapPath(logPath);
            }
            else//非web情况下使用
            {
                logPathFull = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logPath);
            }

            //判断文件目录是否存在
            if (!System.IO.Directory.Exists(logPathFull))
                System.IO.Directory.CreateDirectory(logPathFull);

            string logFile = logPathFull + System.IO.Path.DirectorySeparatorChar.ToString() + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

            //判断文件是否存在
            if (!File.Exists(logFile))
            {
                File.Create(logFile).Close();
            }
            lock (FileObject)
            {
                FileStream fs = new FileStream(logFile, FileMode.Append);
                StringBuilder sw = new StringBuilder();
                sw.AppendLine("**************************************************");
                sw.AppendLine("添加日期：" + sLogDate);
                sw.AppendLine("日志信息：" + content);
                sw.AppendLine("**************************************************");
                byte[] data = new UTF8Encoding().GetBytes(sw.ToString());
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
        }



        #endregion



        #region 20150727
        private static Object LogDebugObject = null;
        private static Object LogInfoObject = null;
        private static Object LogErrorObject = null;

        /// <summary>
        /// debug日志  只在debug环境下输出
        /// </summary>
        /// <param name="content"></param>
        public static void Debug(object content)
        {
            #if DEBUG
            if (LogDebugObject == null)
                LogDebugObject = new object();
            lock (LogDebugObject)
            {
                WriteLog("DEBUG", content);
            }
            #endif
        }
        /// <summary>
        /// debug日志，用户外面控制输出环境
        /// </summary>
        /// <param name="content"></param>
        /// <param name="IsDebug">是否是debug模式</param>
        public static void Debug(string content, bool IsDebug)
        {
            if (IsDebug)
            {
                if (LogDebugObject == null)
                    LogDebugObject = new object();
                lock (LogDebugObject)
                {
                    WriteLog("DEBUG", content);
                }
            }
        }
        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="content"></param>
        public static void Info(object content)
        {
            if (LogInfoObject == null)
                LogInfoObject = new object();
            lock (LogInfoObject)
            {
                WriteLog("INFO", content);
            }
        }
        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="content"></param>
        public static void Error(object content)
        {
            if (LogErrorObject == null)
                LogErrorObject = new object();
            lock (LogErrorObject)
            {
                WriteLog("ERROR", content);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="content"></param>
        private static void WriteLog(string tag, object content)
        {
            DateTime date = DateTime.Now;
            string sLogDate = date.ToString("yyyy-MM-dd HH:mm:ss");
            string logPath = "Log";
            string logPathFull = null;
            if (HttpContext.Current != null)
            {
                logPath = Path.DirectorySeparatorChar + logPath;
                logPathFull = HttpContext.Current.Server.MapPath(logPath);
            }
            else//非web情况下使用
            {
                logPathFull = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logPath);
            }

            //判断文件目录是否存在
            if (!System.IO.Directory.Exists(logPathFull))
                System.IO.Directory.CreateDirectory(logPathFull);

            string logFile = logPathFull + System.IO.Path.DirectorySeparatorChar.ToString() + date.ToString("yyyy-MM-dd") + "-" + tag + ".log";

            //判断文件是否存在
            if (!File.Exists(logFile))
            {
                File.Create(logFile).Close();
            }
            StreamWriter rw = new StreamWriter(logFile, true, System.Text.Encoding.Default);
            rw.WriteLine(string.Format(@"{0},{1} {2} {3}", date.ToString(), date.Millisecond, tag, content));
            rw.Flush();
            rw.Close();
        }
        #endregion
    }
}
