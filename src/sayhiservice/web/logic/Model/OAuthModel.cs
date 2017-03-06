using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logic.Model
{
    /// <summary>
    /// 微信授权后获得用户信息
    /// </summary>
    [Serializable]
    public class WeixinOAuthUserInfoModel
    {
        public string City { get; set; }

        public string Country { get; set; }

        public string Headimgurl { get; set; }

        public string Nickname { get; set; }

        public string Openid { get; set; }

        public string[] Privilege { get; set; }

        public string Province { get; set; }

        public int Sex { get; set; }

        public string UnionID { get; set; }
    }


    /// <summary>
    /// senpac中老版OAuthUserInfo没有unionid，这里暂时扩展
    /// </summary>
    public class OAuthUserInfoPlus
    {
        public OAuthUserInfoPlus()
        { }

        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string nickname { get; set; }
        public string openid { get; set; }
        public string[] privilege { get; set; }
        public string province { get; set; }
        public int sex { get; set; }
        public string unionid { get; set; }
    }



    public class AttendUserModel
    {
        public Int64 rowIndex { get; set; }
        public int id { get; set; }

        public int customerid { get; set; }

        public string openid { get; set; }

        public string nickname { get; set; }

        public string headimgurl { get; set; }

        public string unionId { get; set; }

        public string city { get; set; }

        public string couponCode { get; set; }

        public decimal money { get; set; }
        /// <summary>
        /// 是否参与活动
        /// </summary>
        public int isAttend { get; set; }
        /// <summary>
        /// 默认额度
        /// </summary>
        public decimal defmoney { get; set; }
        /// <summary>
        /// 使用状态0未使用，1已使用
        /// </summary>
        public int useStatus { get; set; }
        /// <summary>
        /// 扫描店铺id
        /// </summary>
        public int shopUserId { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime updateTime { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime useTime { get; set; }


    }


    public class HelpUserModel
    {
        public Int64 rowIndex { get; set; }
        public int id { get; set; }
        public int customerid { get; set; }
        public int UserId { get; set; }
        public decimal money { get; set; }
        public string helpOpenid { get; set; }
        public DateTime createtime { get; set; }
        public string nickname { get; set; }

        public string headimgurl { get; set; }
        public string City { get; set; }

    }


    /// <summary>
    /// 门店
    /// </summary>
    public class ShopUserModel {
        public Int64 rowIndex { get; set; }
        public int id { get; set; }
        public int customerid { get; set; }

        public string loginName { get; set; }

        public string loginPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime createtime { get; set; }
        /// <summary>
        /// 经销商
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 微信绑定的openid
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 微信默认绑定的的账号
        /// </summary>
        public int defStatus { get; set; }
        /// <summary>
        /// 专卖店大区
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 专卖店省份
        /// </summary>
        public string pro { get; set; }
        /// <summary>
        /// 专卖店区域
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 专卖店名称
        /// </summary>
        public string shopName { get; set; }
        /// <summary>
        /// 专卖店地址
        /// </summary>
        public string shopAddress { get; set; }


    }

    /// <summary>
    /// 门店搜索
    /// </summary>
    public class ShopSearchWhere {
        public string key { get; set; }

        public string area { get; set; }

        public string pro { get; set; }

        public string city { get; set; }
    }

    public class CouponSearchWhere {
        public string key { get; set; }

        public int id { get; set; }

        public int useStatus { get; set; }

        public string startTime { get; set; }

        public string endTime { get; set; }
    }



    public class WxAccessTokenTicket {
        public int id { get; set; }

        public int customerid { get; set; }

        public string value { get; set; }

        public DateTime gettime { get; set; }
    }
}

