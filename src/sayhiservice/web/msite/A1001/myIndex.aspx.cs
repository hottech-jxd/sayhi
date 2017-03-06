using common;
using logic;
using logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace msite.A1001
{
    /// <summary>
    /// 我的优惠券
    /// </summary>
    public partial class myIndex : PageBase
    {
        protected AttendUserModel userModel = null;
        /// <summary>
        /// 
        /// </summary>
        protected string PageTitle = "";
        protected int attendCount = 0;
        /// <summary>
        /// 判断是否是自己访问自己的页面
        /// </summary>
        protected bool _self = true;
        /// <summary>
        /// 发起人ID
        /// </summary>
        protected string shareOpenId
        {
            get
            {
                return this.GetQueryString("shareopenid", "");
            }
        }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        protected string actEndTime
        {
            get
            {
                return ConfigHelper.GetConfigString("endTime", "2016-05-02");
            }
        }



        protected string ShowText { get; set; }




        /// <summary>
        /// 当前访问用户的ID
        /// </summary>
        protected string currentOpenid
        {
            get
            {
                return this.GetOpenIdVal();
            }
        }
        /// <summary>
        /// 活动是否结束
        /// </summary>
        protected bool isEnd { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(shareOpenId))
            {
                WriteLog("invalid URL");
            }

            DateTime outime;
            DateTime.TryParse(actEndTime, out outime);
            if (DateTime.Now < outime)
                isEnd = false;
            else
                isEnd = true;

            if (currentOpenid == shareOpenId)//当前是自己访问自己的页面
            {
                userModel = UserService.Instance.GetAttendUserInfo(CurrentCustomerID, currentOpenid);
                _self = true;
                if (userModel != null)
                {

                    if (userModel.isAttend == 0 && !isEnd)
                    {
                        //修改用户参与状态
                        UserService.Instance.UpdateAttendUserStatus(CurrentCustomerID, currentOpenid);
                    }

                    if (userModel.isAttend == 0 && isEnd)
                    {
                        WriteLog("活动已结束");
                    }


                    if (userModel.useStatus == 0)
                        this.ShowText = "朋友已帮 <span style='color:red'>wo</span> 砍价" + (userModel.money - userModel.defmoney) + "元";
                    else
                        this.ShowText = "您的优惠券已使用！";

                }
            }
            else
            {
                _self = false;
                userModel = UserService.Instance.GetAttendUserInfo(CurrentCustomerID, shareOpenId);
                if (userModel != null)
                {
                    if (userModel.useStatus == 0)
                        this.ShowText = "<span style='color:red'>" + userModel.nickname + "</span>邀请您帮TA砍价";
                    else
                        this.ShowText = "<span style='color:red'>" + userModel.nickname + "</span>的优惠券已使用";
                }
            }

            if (userModel != null)
            {
                myCouponCode.Value = userModel.couponCode;
                attendCount = UserService.Instance.GetAttendActCount(CurrentCustomerID, userModel.id);
            }
        }


        public void GetHelpUserList()
        {

        }





        private void WriteLog(string str)
        {
            Response.Redirect("/error/index.aspx?t=" + str);
        }

    }
}