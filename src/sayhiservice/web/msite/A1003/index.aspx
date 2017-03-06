<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="msite.A1003.index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1,user-scalable=no,maximum-scale=1,initial-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <script src="/js/jquery-1.7.2.min.js"></script>
    <title>伊力特线上代理商申请表格</title>
    <meta name="description" content="伊力特线上代理商申请表格" />
    <link href="../css/common.css?v=1.0" rel="stylesheet" />
</head>
<style>
    body, html {
        background-color: #ffa600;
    }

    select {
        /*Chrome和Firefox里面的边框是不一样的，所以复写了一下*/
        border: solid 1px #ffffff;
        /*很关键：将默认的select选择框样式清除*/
        appearance: none;
        -moz-appearance: none;
        -webkit-appearance: none;
        /*在选择框的最右侧中间显示小箭头图片*/
        background: url("/images/arrow.png") no-repeat scroll right center transparent;
        /*为下拉小箭头留出一点位置，避免被文字覆盖*/
        background-color: #ffffff;
    }


        /*清除ie的默认选择框样式清除，隐藏下拉箭头*/
        select::-ms-expand {
            display: none;
        }
</style>
<body>
    <div style="display: none;">
        <img src="/images/1003/logo.jpg" />
    </div>
    <form id="form1" runat="server">
        <div>
            <img class="gridlazy" src="/images/1003/top.jpg" style="width: 100%;" />
        </div>
        <div>
            <img class="gridlazy" src="/images/1003/1234.png" style="width: 100%;" />
        </div>
        <p style="clear: both"></p>
        <div class="content2">
            <div class="content-title3" style="background-color: #fd6600;">
                代理商申请报名
            </div>
            <div class="form">
                <p>
                    <label>姓&nbsp;名：</label>
                    <input type="text" class="form_text" id="txtName" placeholder="请输入您的真实姓名" value="<%=data!=null?data.name:"" %>" />
                </p>
                <p>
                    <label>手&nbsp;机：</label>
                    <input type="text" class="form_text" id="txtMobile" placeholder="请输入您的手机号码" value="<%=data!=null?data.mobile:"" %>" />
                </p>
                <p>
                    <label>店&nbsp;名：</label>
                    <input type="text" class="form_text" id="txtContent1" placeholder="请输入您的店名" value="<%=data!=null?data.content1:"" %>" />
                </p>

                <p>
                    <label>省&nbsp;份：</label>
                    <select class="form_select" id="s_province" name="s_province" disabled="disabled">
                    </select>
                </p>
                <p>
                    <label>城&nbsp;市：</label>
                    <select class="form_select" id="s_city" name="s_city"></select>
                </p>
                <p>
                    <label>县&nbsp;区：</label>
                    <select class="form_select" id="s_county" name="s_county"></select>
                </p>
                <p>
                    <label>办事处：</label>
                    <select class="form_select" id="txtContent3">
                        <option value="" selected="selected">请选择对应办事处</option>
                        <option value="杭一区">杭一区</option>
                        <option value="杭二区">杭二区</option>
                        <option value="杭州流通">杭州流通</option>
                        <option value="嘉兴办事处">嘉兴办事处</option>
                        <option value="湖州办事处">湖州办事处</option>
                        <option value="绍兴办事处">宁波办事处</option>
                        <option value="舟山办事处">舟山办事处</option>
                        <option value="金华办事处">金华办事处</option>
                        <option value="台州办事处">台州办事处</option>
                        <option value="温州办事处">温州办事处</option>
                    </select>
                </p>
                <p>
                    <label>业务员：</label>
                    <input type="text" class="form_text" id="txtContent4" placeholder="请输入业务员姓名及电话，不详写“无” " value="<%=data!=null?data.content4:"" %>" />
                </p>
                <p>
                    <label>备&nbsp;注：</label>
                    <input type="text" class="form_text" id="txtContent2" placeholder="备注 " value="<%=data!=null?data.content2:"" %>" />
                </p>
            </div>
            <div class="btn1" onclick="btnSubmit()" style="background-color: #fd6600;">
                申请报名
            </div>

            <p></p>
        </div>

        <div style="height: 30px;"></div>
    </form>
</body>
</html>
<script class="resources library" src="/js/jquery.area.js" type="text/javascript"></script>
<script src="/js/loading/jquery.loading-0.1.js"></script>
<link href="/js/layer/need/layer.css" rel="stylesheet" />
<script src="/js/layer/layer.js"></script>
<script type="text/javascript">

    var isSignUp = '<%=IsSignUp%>';
    var s_province = "<%=data!=null?data.Province:"省份" %>",
    s_city = "<%=data!=null?data.city:"请选择对应城市" %>"
    s_county = "<%=data!=null?data.address:"请选择对应县区" %>";
    $(function () {
        _init_area();
        $("#s_province").val("浙江省").change();
        $("#s_city").val(s_city).change();
        $("#s_county").val(s_county).change();

    });


    function checkForm() {
        if ($("#txtName").val().length == 0) {
            layer.open({
                content: '请输入您的真实姓名',
                time: 2 //2秒后自动关闭
            });
            return false;
        }

        var tel = $("#txtMobile").val();
        var reg = /^0?1[2|3|4|5|7|8][0-9]\d{8}$/;
        if (!reg.test(tel)) {
            layer.open({
                content: '请输入您的手机号码',
                time: 2 //2秒后自动关闭
            });
            return false;
        }

        if ($("#txtContent1").val().length == 0) {
            layer.open({
                content: '请输入您的店名',
                time: 2 //2秒后自动关闭
            });
            return false;
        }

        if ($("#s_province").val().length == 0 || $("#s_province").val() == "省份") {
            layer.open({
                content: '请选择省份',
                time: 2 //2秒后自动关闭
            });
            return false;
        }
        if ($("#s_city").val().length == 0 || $("#s_city").val() == "请选择对应城市") {
            layer.open({
                content: '请选择对应城市',
                time: 2 //2秒后自动关闭
            });
            return false;
        }
        if ($("#s_county").val().length == 0 || $("#s_county").val() == "请选择对应县区") {
            layer.open({
                content: '请选择对应县区',
                time: 2 //2秒后自动关闭
            });
            return false;
        }
        if ($("#txtContent3").val().length == 0) {
            layer.open({
                content: '请选择对应办事处',
                time: 2 //2秒后自动关闭
            });
            return false;
        }

        //
        return true;
    }

    //报名
    function btnSubmit() {
        //if (isSignUp == "1") {
        //    layer.open({
        //        content: "您已经申请过了，无需重复申请！",
        //        time: 2 //2秒后自动关闭
        //    });
        //    return
        //}
        if (!checkForm()) return;

        loading.show();
        $.ajax({
            type: "post",
            url: "/AjaxHandler.aspx",
            data: {
                action: "signUp",
                name: $("#txtName").val(),
                mobile: $("#txtMobile").val(),
                province: $("#s_province").val(),
                city: $("#s_city").val(),
                county: $("#s_county").val(),
                customerid: "<%=this.CurrentCustomerID%>",
                type: 0,
                content1: $("#txtContent1").val(),
                content2: $("#txtContent2").val(),
                content3: $("#txtContent3").val(),
                content4: $("#txtContent4").val()
            },
            success: function (ret) {
                loading.close();
                if (ret.code == 1) {
                    window.location.href = "success.html";
                }
                else {
                    layer.open({
                        content: "很抱歉！您申请失败！",
                        time: 2 //2秒后自动关闭
                    });
                }
            },
            error: function (err) {
                layer.open({
                    content: "很抱歉！您申请失败！",
                    time: 2 //2秒后自动关闭
                });
                loading.close();
            }
        });
    }


</script>
