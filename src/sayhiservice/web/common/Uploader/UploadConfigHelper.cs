using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace common
{
    public class UploadConfigHelper
    {
        private static UploadConfigHelper _Current;
        private static List<UploadConfigInfo> _List = new List<UploadConfigInfo>();
        static UploadConfigHelper()
        {
            _Current = new UploadConfigHelper();
        }
        private UploadConfigHelper()
        {
            LoadUploadConfig();
        }
        public static UploadConfigHelper Current
        {
            get { return _Current; }
        }

        #region 根据Key读取相应的上传文件的配置
        /// <summary>
        /// 根据Key读取相应的上传文件的配置[UploadConfigInfo]
        /// </summary>
        /// <param name="sKey">键值</param>
        /// <returns></returns>
        public UploadConfigInfo GetUploadConfigInfoByKey(string sKey)
        {
            if (_List != null && _List.Count > 0)
            {
                return (from p in _List
                        where p.Key == sKey
                        select p).FirstOrDefault();
                //return _List.Find(new Predicate<UploadConfigInfo>(delegate(UploadConfigInfo model)
                //{
                //    return model.Key.ToLower().Equals(sKey.ToLower());
                //}));
            }
            else
            {
                return null;
            }
        }

        //private UploadConfigInfo GetDefaultUploadConfigInfo()
        //{
        //    UploadConfigInfo model = new UploadConfigInfo();
        //    return model;
        //}
        #endregion

        #region 加载上传文件的配置
        private void LoadUploadConfig()
        {
            string sFile = GetUploadConfigFile();
            if (string.IsNullOrEmpty(sFile))
                return;
            var ConfigData = from p in XElement.Load(HttpContext.Current.Server.MapPath(sFile)).Descendants("File")
                             select p;
            foreach (XElement xe in ConfigData)
            {
                UploadConfigInfo model = new UploadConfigInfo();
                model.Key = xe.Attribute("Key").Value;
                model.URL = xe.Attribute("URL").Value;
                model.IsImg = xe.Attribute("IsImg").Value == "true";
                model.FileNamePrefix = xe.Attribute("FileNamePrefix").Value;
                model.FileExt = xe.Attribute("FileExt").Value;
                model.MaxSize = xe.Attribute("MaxSize") == null ? 0 : Convert.ToInt32(xe.Attribute("MaxSize").Value);
                model.ToThumbnail = xe.Attribute("ToThumbnail") == null ? false : (xe.Attribute("ToThumbnail").Value == "true");
                model.Width = xe.Attribute("Width") == null ? 0 : Convert.ToInt32(xe.Attribute("Width").Value);
                model.Height = xe.Attribute("Height") == null ? 0 : Convert.ToInt32(xe.Attribute("Height").Value);
                if (xe.Attribute("ThumbnailMode") != null && xe.Attribute("ThumbnailMode").Value != "")
                {
                    model.ThumbnailMode = (ThumbnailMode)Enum.Parse(typeof(ThumbnailMode), xe.Attribute("ThumbnailMode").Value, true);
                }
                if (xe.Attribute("OtherThumbnail") != null)
                {
                    model.OtherThumbnail = xe.Attribute("OtherThumbnail").Value;
                }
                else
                {
                    model.OtherThumbnail = "";
                }
                model.MaxModeSize = xe.Attribute("MaxModeSize") == null ? 0 : Convert.ToInt32(xe.Attribute("MaxModeSize").Value);
                if (xe.Attribute("MaxMode") != null && xe.Attribute("MaxMode").Value != "")
                {
                    model.MaxMode = (ThumbnailMode)Enum.Parse(typeof(ThumbnailMode), xe.Attribute("MaxMode").Value, true);
                }
                model.ToWater = xe.Attribute("ToWater") == null ? false : (xe.Attribute("ToWater").Value == "true");
                if (xe.Attribute("WaterPosition") != null && xe.Attribute("WaterPosition").Value != "")
                {
                    model.WaterPosition = (WaterPositionOptions)Enum.Parse(typeof(WaterPositionOptions), xe.Attribute("WaterPosition").Value, true);
                }
                _List.Add(model);
            }
        }
        #endregion

        #region 获得上传文件配置文件
        /// <summary>
        /// 获得上传文件配置文件
        /// </summary>
        /// <returns></returns>
        private string GetUploadConfigFile()
        {
            return ConfigHelper.GetConfigString("UploadConfig", "~/config/UploadConfig.xml");
        }
        #endregion
    }
}
