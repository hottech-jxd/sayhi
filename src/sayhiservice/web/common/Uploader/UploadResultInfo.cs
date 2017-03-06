using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common
{
    public enum UploadFileType
    {
        File, Img, Flash
    }

    public class UploadResultInfo
    {
        public UploadResultInfo()
        {
            FileType = UploadFileType.File;
            UploadStatus = true;
            Msg = "文件上传成功！";
        }

        /// <summary>
        /// 上传结果
        /// </summary>
        public bool UploadStatus { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件全名  路径+文件名
        /// </summary>
        public string FullFileName { get; set; }

        /// <summary>
        /// 文件类型  File, Img, Flash
        /// </summary>
        public UploadFileType FileType { get; set; }

        /// <summary>
        /// 图片宽度
        /// </summary>
        public int ImgWidth { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        public int ImgHeight { get; set; }
        
    }
}
