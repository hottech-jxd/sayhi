using common;
using logic;
using logic.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace admin_modeng
{

    /// <summary>
    /// Api 的摘要说明
    /// </summary>
    [WebService(
        Namespace = "http://modeng.com",
        Name = "摩登人家接口文档",
        Description =
        "<p>公司名称：杭州火图科技有限公司</p>" +
        "<p>技术支持：郭孟稳</p>" +
        "<p>固定参数：timestamp、token(用户唯一标识)</p>" +
        "<p>签名方式：参数名全部小写,按升序排序，进行md5_utf8(key=value&key=value...AppSecrect)加密签名</p>" +
        "<p>请求方式：POST</p>" +
        "<p>注意事项：接口名区分大小写</p>"


        + "<br/>"
        )
    ]

    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class Api : BaseService
    {
        [WebMethod(Description = "<br/><b>获得我的申请记录</b><p>token:用户唯一标识<br/>type:0贷款 1保险<br/>pageIndex:页码</p>")]
        [ScriptMethod]
        public void getMyApply(string token, int type, int pageIndex = 1)
        {
            VerifySignCheck();
            try
            {

                pageIndex = pageIndex > 0 ? pageIndex : 1;
                SearchWhere where = new SearchWhere();
                where.token = token;
                where.type = type;
                result.index = pageIndex;
                List<UserCarInfoModel> data = UserCarLogic.Instance.GetUserCarList(pageIndex, pageSize, out recordCount, where);
                if (data != null)
                {
                    data.ForEach((item) =>
                    {
                        item.StatusName = item.Status == 1 ? "通过" : item.Status == -1 ? "失败" : item.StatusName;
                        item.Remark = string.IsNullOrEmpty(item.Remark) ? "" : item.Remark;
                    });

                    result.setResult(ApiStatusCode.OK, data);
                }
            }
            catch (SoapException ex)
            {
                LogHelper.Debug(string.Format("StackTrace:{0},Message:{1}", ex.StackTrace, ex.Message), this.DebugMode);
                result.setResult(ApiStatusCode.服务器错误, ex.Message);
            }
            JsonResult(result);
        }


        [ScriptMethod]
        [WebMethod(Description = "提交申请贷款")]
        public void submitApplyLoan(string token, string age, string carInfo, string carType, string city, string repaymentPeriod, int storeId, string invoicePrice, string loanMoney)
        {
            VerifySignCheck();
            try
            {
                if (storeId <= 0)
                {
                    result.setResult(ApiStatusCode.参数错误, "缺少客户资料");
                    goto Finish;
                }

                //添加贷款
                int flag = UserCarLogic.Instance.AddUserCar(new UserCarInfoModel()
                {
                    ApplyAge = age,
                    carInfo = carInfo,
                    carType = carType,
                    City = city,
                    CustomerId = storeId,
                    invoicePrice = invoicePrice,
                    LoanMoney = loanMoney,
                    RepaymentPeriod = repaymentPeriod,
                    Usertoken = token,
                    WIE = 0,
                    Status = 0,
                    Type = 0,
                    FristBeneficiary = string.Empty,
                    InsuranceType = string.Empty,
                    agreement = string.Empty,
                    OrderNo = DateTime.Now.ToString("yyyyMMddHHmmss") + StringHelper.CreateCheckCodeWithNum(4)
                });
                if (flag > 0)
                    result.setResult(ApiStatusCode.OK, flag);
                else
                    result.setResult(ApiStatusCode.失败, "");
            }
            catch (Exception ex)
            {
                LogHelper.Debug(string.Format("StackTrace:{0},Message:{1}", ex.StackTrace, ex.Message), this.DebugMode);
                result.setResult(ApiStatusCode.服务器错误, ex.Message);
            }
            goto Finish;
        Finish:
            JsonResult(result);

        }


        [ScriptMethod]
        [WebMethod(Description = "提交申请保险")]
        public void submitApplyInsurance(string token, string carType, string city, int storeId, int wie, string invoicePrice, string fristBeneficiary, string insuranceType, string agreement)
        {
            VerifySignCheck();
            try
            {
                if (storeId <= 0)
                {
                    result.setResult(ApiStatusCode.参数错误, "缺少客户资料");
                    goto Finish;
                }
                //添加贷款
                int flag = UserCarLogic.Instance.AddUserCar(new UserCarInfoModel()
                {
                    carType = string.IsNullOrEmpty(carType) ? "" : carType,
                    City = string.IsNullOrEmpty(city) ? "" : city,
                    CustomerId = storeId,
                    invoicePrice = string.IsNullOrEmpty(invoicePrice) ? "" : invoicePrice,
                    FristBeneficiary = string.IsNullOrEmpty(fristBeneficiary) ? "" : fristBeneficiary,
                    InsuranceType = string.IsNullOrEmpty(insuranceType) ? "" : insuranceType,
                    agreement = string.IsNullOrEmpty(agreement) ? "" : agreement,
                    Usertoken = token,
                    WIE = wie,
                    carInfo = "",
                    ApplyAge = "",
                    LoanMoney = "",
                    RepaymentPeriod = "",
                    Status = 0,
                    Type = 1,
                    OrderNo = DateTime.Now.ToString("yyyyMMddHHmmss") + StringHelper.CreateCheckCodeWithNum(4)
                });
                if (flag > 0)
                    result.setResult(ApiStatusCode.OK, "");
                else
                    result.setResult(ApiStatusCode.失败, "");
            }
            catch (Exception ex)
            {
                LogHelper.Debug(string.Format("StackTrace:{0},Message:{1}", ex.StackTrace, ex.Message), this.DebugMode);
                result.setResult(ApiStatusCode.服务器错误, ex.Message);
            }
            goto Finish;
        Finish:
            JsonResult(result);
        }

        /// <summary>
        /// 提交客户资料
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mobile"></param>     
        [ScriptMethod]
        [WebMethod(Description = "提交客户资料")]
        public void submitCustomerInfo()
        {
            VerifySignCheck();
            try
            {
                int storeId = prams.Value<int>("storeId");
                string name = prams.Value<string>("name");
                string mobile = prams.Value<string>("mobile");
                string token = prams.Value<string>("token");
                int type = prams.Value<int>("type");
                if (string.IsNullOrEmpty(name))
                {
                    result.setResult(ApiStatusCode.参数错误, "客户名称不能为空");
                    goto Finish;
                }
                if (string.IsNullOrEmpty(mobile))
                {
                    result.setResult(ApiStatusCode.参数错误, "客户联系不能为空");
                    goto Finish;
                }
                if (!RegexHelper.IsValidRealMobileNo(mobile))
                {
                    result.setResult(ApiStatusCode.参数错误, "非法的手机号码");
                    goto Finish;
                }


                string imgContent = "";
                //上传图片
                for (int i = 0; i < Context.Request.Files.Count; i++)
                {
                    HttpPostedFile oFile = Context.Request.Files[i];
                    if (oFile == null) continue;
                    string fileName = "/resource/modeng/" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + StringHelper.CreateCheckCodeWithNum(6) + ".jpg";
                    if (FileUploadHelper.UploadFile(oFile, fileName))
                    {
                        if (string.IsNullOrEmpty(imgContent))
                            imgContent = fileName;
                        else
                            imgContent += "|" + fileName;
                    }
                }
                int flag = UserCarLogic.Instance.AddCustomerModel(new CustomerModel()
                {
                    UserId = storeId,
                    UserName = name,
                    UserMobile = mobile,
                    UserImg = imgContent,
                    UserToken = string.IsNullOrEmpty(token) ? "" : token,
                    UserType = type
                });
                if (flag > 0)
                    result.setResult(ApiStatusCode.OK, flag);
                else
                    result.setResult(ApiStatusCode.失败, "");
            }
            catch (Exception ex)
            {
                LogHelper.Debug(string.Format("StackTrace:{0},Message:{1}", ex.StackTrace, ex.Message), this.DebugMode);
                result.setResult(ApiStatusCode.服务器错误, ex.Message);
            }
            goto Finish;
        Finish:
            JsonResult(result);
        }
        /// <summary>
        /// 根据客户id获取客户资料
        /// </summary>   
        [ScriptMethod]
        [WebMethod(Description = "根据客户id获取客户资料")]
        public void getCustomerInfo(int storeId)
        {
            VerifySignCheck();
            try
            {
                if (storeId <= 0)
                {
                    result.setResult(ApiStatusCode.参数错误, "客户Id丢失");
                    goto Finish;
                }

                CustomerModel model = UserCarLogic.Instance.GetCustomerModel(storeId);
                if (model == null)
                {
                    result.setResult(ApiStatusCode.失败, "");
                    goto Finish;
                }

                AppCustomerModel data = new AppCustomerModel()
                {
                    name = model.UserName,
                    mobile = model.UserMobile,
                    storeId = model.UserId
                };
                if (!string.IsNullOrEmpty(model.UserImg))
                {
                    string[] arr = model.UserImg.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] imgData = new string[arr.Length];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string imgUrl = "http://" + Context.Request.Url.Authority + arr[i];
                        imgData[i] = imgUrl;
                    }
                    data.imgData = imgData;
                }
                result.setResult(ApiStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(string.Format("StackTrace:{0},Message:{1}", ex.StackTrace, ex.Message), this.DebugMode);
                result.setResult(ApiStatusCode.服务器错误, ex.Message);
            }
            goto Finish;
        Finish:
            JsonResult(result);
        }

        [ScriptMethod]
        [WebMethod(Description = "<br/><b>获取客户资料</b><p>token:用户唯一标识<br/>type:0贷款 1保险<br/>pageIndex:页码</p>")]
        public void getCustomerList(string token, int type, int pageIndex)
        {
            VerifySignCheck();
            try
            {
                pageIndex = pageIndex > 0 ? pageIndex : 1;
                SearchWhere where = new SearchWhere();
                where.token = token;
                where.type = type;
                result.index = pageIndex;
                var data = UserCarLogic.Instance.GetCusstomerList(pageIndex, pageSize, out recordCount, where);
                if (data != null)
                {
                    result.setResult(ApiStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug(string.Format("StackTrace:{0},Message:{1}", ex.StackTrace, ex.Message), this.DebugMode);
                result.setResult(ApiStatusCode.服务器错误, ex.Message);
            }
            goto Finish;
        Finish:
            JsonResult(result);
        }



    }
}
