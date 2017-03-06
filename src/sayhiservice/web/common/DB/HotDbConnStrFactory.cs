using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace common.DB
{
    /// <summary>
    /// 数据库链接字符串提供器
    /// 2014-11-12讨论数据库分离，粉猫用到的几个表(商户基础表)留在原库，目前如下:
    /// =====================================================
    /// [商户基本信息表]
    ///  - Swt_CustomerManage,Swt_IndustryManage,
    /// [商户功能模块设置表]
    ///  - FM_SystemFunctionConfig,FM_CustomerFunctionConfig
    /// [管理员表]
    ///  - Swt_Manager,Swt_RoleManage,Swt_RoleLimitManage
    /// [微信配置表]
    ///  - Swt_WechatConfigCore,Swt_KeywordReplyRule,Swt_MicroReplyType,Swt_ReplyContentManage,Swt_CustomMenuManage,Sys_WebSiteAppConfig
    /// =====================================================
    /// </summary>
    public class HotDbConnStrFactory
    {
        private static HotDbConnStrFactory factory = new HotDbConnStrFactory();
        private HotDbConnStrFactory()
        { }

        /// <summary>
        /// 单例出口
        /// </summary>
        public static HotDbConnStrFactory Instance
        {
            get
            {
                return factory;
            }
        }

        /// <summary>
        /// 商户基本信息主库—链接字符串，不配置或留空情况下，使用相同的链接字符串
        /// </summary>
        public string CustomerMainConnectString
        {
            get
            {
                string connstr = ConfigHelper.GetConfigString("MssqlDBConnectionString_CustomerMain", ConfigHelper.MssqlDBConnectionString);
                return connstr == "" ? ConfigHelper.MssqlDBConnectionString : connstr;
            }
        }

        /// <summary>
        /// 商户基本信息主库—链接服务器名称，如linkname.dbname.dbo
        /// </summary>
        public string CustomerMainLinkedServerName
        {
            get
            {
                //商户基本信息主库未配置的，链接服务器肯定也启用，这里判断下
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["MssqlDBConnectionString_CustomerMain"]))
                {
                    return "";
                }
                string serverName = ConfigHelper.GetConfigString("CustomerMainLinkedServerName", "");
                if (serverName != "")
                {
                    return serverName.TrimEnd('.') + ".";
                }
                return "";
            }
        }
    }
}
