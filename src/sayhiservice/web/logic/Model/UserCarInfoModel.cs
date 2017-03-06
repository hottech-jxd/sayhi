using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logic.Model
{
    /// <summary>
    /// 车辆信息
    /// </summary>
    [Serializable]
    public class UserCarInfoModel
    {
        #region Model
        private int _id;
        private int _type;
        private string _usertoken;
        private string _carinfo;
        private string _cartype;
        private string _invoiceprice;
        private string _insurancetype;
        private string _fristbeneficiary;
        private string _agreement;
        private int _wie = 0;
        private string _city;
        private int _customerid;
        private int _status = 0;
        private string _applyage;
        private string _loanmoney;
        private string _repaymentperiod;
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// id
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 0贷款  1保险
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 用户唯一值
        /// </summary>
        public string Usertoken
        {
            set { _usertoken = value; }
            get { return _usertoken; }
        }
        /// <summary>
        /// 车辆信息
        /// </summary>
        public string carInfo
        {
            set { _carinfo = value; }
            get { return _carinfo; }
        }
        /// <summary>
        /// 车型
        /// </summary>
        public string carType
        {
            set { _cartype = value; }
            get { return _cartype; }
        }
        /// <summary>
        /// 开票价格
        /// </summary>
        public string invoicePrice
        {
            set { _invoiceprice = value; }
            get { return _invoiceprice; }
        }
        /// <summary>
        /// 保险险种
        /// </summary>
        public string InsuranceType
        {
            set { _insurancetype = value; }
            get { return _insurancetype; }
        }
        /// <summary>
        /// 第一受益人
        /// </summary>
        public string FristBeneficiary
        {
            set { _fristbeneficiary = value; }
            get { return _fristbeneficiary; }
        }
        /// <summary>
        /// 特别约定
        /// </summary>
        public string agreement
        {
            set { _agreement = value; }
            get { return _agreement; }
        }
        /// <summary>
        /// 是否立即生效 0=否 1=是
        /// </summary>
        public int WIE
        {
            set { _wie = value; }
            get { return _wie; }
        }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustomerId
        {
            set { _customerid = value; }
            get { return _customerid; }
        }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string storeName { get; set; }
        /// <summary>
        /// 申请状态
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }

        private string _statusName = "申请中";
        /// <summary>
        /// 申请状态
        /// </summary>
        public string StatusName
        {
            get { return _statusName; }
            set { _statusName = value; }
        }

        /// <summary>
        /// 申请了年龄
        /// </summary>
        public string ApplyAge
        {
            set { _applyage = value; }
            get { return _applyage; }
        }
        /// <summary>
        /// 贷款金额
        /// </summary>
        public string LoanMoney
        {
            set { _loanmoney = value; }
            get { return _loanmoney; }
        }
        /// <summary>
        /// 还款年限
        /// </summary>
        public string RepaymentPeriod
        {
            set { _repaymentperiod = value; }
            get { return _repaymentperiod; }
        }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        public string Remark { get; set; }

        #endregion Model
    }


    /// <summary>
    /// 客户资料
    /// </summary>
    [Serializable]
    public class CustomerModel
    {
        #region Model
        private int _userid;
        private string _usertoken;
        private string _username;
        private string _usermobile;
        private int _usersex;
        private string _useridcard;
        private string _userimg;
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// 客户ID
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 用户唯一值
        /// </summary>
        public string UserToken
        {
            set { _usertoken = value; }
            get { return _usertoken; }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string UserMobile
        {
            set { _usermobile = value; }
            get { return _usermobile; }
        }
        /// <summary>
        /// 用户性别
        /// </summary>
        public int UserSex
        {
            set { _usersex = value; }
            get { return _usersex; }
        }
        /// <summary>
        /// 用户身份证号
        /// </summary>
        public string UserIDCard
        {
            set { _useridcard = value; }
            get { return _useridcard; }
        }
        /// <summary>
        /// 用户附件图片
        /// </summary>
        public string UserImg
        {
            set { _userimg = value; }
            get { return _userimg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 0贷款  1保险
        /// </summary>
        public int UserType { get; set; }

        #endregion Model
    }

    [Serializable]
    public class AppCustomerModel
    {
        #region Model
        private int _userid;
        private string _username;
        private string _usermobile;
        private string[] _imgdata = { };
        /// <summary>
        /// 客户ID
        /// </summary>
        public int storeId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string name
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string mobile
        {
            set { _usermobile = value; }
            get { return _usermobile; }
        }
        /// <summary>
        /// 图片数据
        /// </summary>
        public string[] imgData
        {
            set { _imgdata = value; }
            get { return _imgdata; }
        }
        #endregion Model
    }
    [Serializable]
    public class AppCustomerImgModel
    {
        public string url { get; set; }
    }


    /// <summary>
    /// 用户搜索
    /// </summary>
    public class SearchWhere
    {
        public string key { get; set; }
        public int type { get; set; }
        public string token { get; set; }
        /// <summary>
        /// -1 失败 0 申请中 1申请通过
        /// </summary>
        public int Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }

        private int _status = -2;

    }



    /// <summary>
    ///Api提交客户资料
    /// </summary>
    [Serializable]
    public class ApiCustomerModel
    {
        #region Model
        private string _usertoken;
        private string _username;
        private string _usermobile;
        private string _userimg;

        /// <summary>
        /// 用户唯一值
        /// </summary>
        public string UserToken
        {
            set { _usertoken = value; }
            get { return _usertoken; }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string UserMobile
        {
            set { _usermobile = value; }
            get { return _usermobile; }
        }

        /// <summary>
        /// 用户附件图片
        /// </summary>
        public string UserImg
        {
            set { _userimg = value; }
            get { return _userimg; }
        }
        #endregion Model
    }


}
