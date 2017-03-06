using common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msite.OAuth2
{
    public class MPHelper
    {

        /// <summary>
        /// 获取微信用户基本信息（包括UnionID机制）
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static MPUserInfoResult GetWxUserInfo(string wxApiUrl, out string errmsg)
        {
            errmsg = "";
            try
            {
                IHttpForm _httpForm = new HttpForm("HOT-MPHelper", 15000, true, 8);
                HttpFormResponse _response = _httpForm.Get(new HttpFormGetRequest()
                {
                    Url = wxApiUrl
                });
                MPUserInfoResult result = JsonConvert.DeserializeObject<MPUserInfoResult>(_response.Response);
                return result;

            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                LogHelper.Write("获取微信用户基本信息(GetWxUserInfo)异常 -->:" + ex.Message);
                return null;
            }

        }
    }


    /// <summary>
    /// 微信用户基本信息（包括UnionID机制）
    /// </summary>
    [Serializable]
    public class MPUserInfoResult
    {
        /// <summary>
        /// 0表示正常
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public int subscribe { get; set; }
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户特权信息json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public string[] privilege { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public long subscribe_time { get; set; }
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段
        /// </summary>
        public string unionid { get; set; }
        /// <summary>
        /// 公众号运营者对粉丝的备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        public int groupid { get; set; }
    }

}