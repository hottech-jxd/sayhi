using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common
{
    public class WaterPosition
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
    public enum ThumbnailMode
    {
        /// <summary>
        /// 按指定宽高适应缩小
        /// </summary>
        HW = 0,
        /// <summary>
        /// 按宽度等比缩小
        /// </summary>
        W = 1,
        /// <summary>
        /// 按高度等比缩小
        /// </summary>
        H = 2,
        /// <summary>
        /// 按指定宽高等比剪切缩小
        /// </summary>
        Cut = 3,
        Ration = 4,
        /// <summary>
        /// 按指定宽高填充（不剪切）缩小
        /// </summary>
        Fill = 5,
        Orgion
    }
    public enum WaterPositionOptions
    {
        LeftTop = 0,
        RightTop = 1,
        Middle = 2,
        LeftBottom = 3,
        RightBottom = 4
    }
    public class UploadConfigInfo : ICloneable
    {
        public string Key { get; set; }
        public string URL { get; set; }
        public bool IsImg { get; set; }
        public string FileNamePrefix { get; set; }
        public string FileExt { get; set; }
        public int MaxSize { get; set; }
        public bool ToThumbnail { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ThumbnailMode ThumbnailMode { get; set; }
        public string OtherThumbnail { get; set; }
        public int MaxModeSize { get; set; }
        public ThumbnailMode MaxMode { get; set; }
        public bool ToWater { get; set; }
        public WaterPositionOptions WaterPosition { get; set; }


        #region ICloneable 成员
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }
    
}
