<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="freeIndex.aspx.cs" Inherits="msite.A1002.freeIndex" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1,user-scalable=no,maximum-scale=1,initial-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <script src="/js/jquery-1.7.2.min.js"></script>
    <title>预约0元设计</title>
    <link href="../css/common.css?v=1.0" rel="stylesheet" />
</head>
<style>
    body {
        background-color: #56090f;
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
    <form id="form1" runat="server">
        <div>
            <img class="gridlazy" src="/images/main-01_01.jpg" style="width: 100%;" />
        </div>

        <div style="text-align: center;">
            <ul style="padding: 0px;">
                <li style="float: left; width: 33%">
                    <img src="/images/1002/1.jpg" style="width: 100%" /></li>
                <li style="float: left; width: 33%">
                    <img src="/images/1002/2.jpg" style="width: 100%" /></li>
                <li style="float: left; width: 33%">
                    <img src="/images/1002/3.jpg" style="width: 100%" /></li>
            </ul>
        </div>

        <p style="clear: both"></p>

        <div class="content2">
            <div class="content-title3">
                预约登记
            </div>
            <div class="form">
                <p>
                    <label>姓名：</label>
                    <input type="text" class="form_text" id="txtName" placeholder="请输入您的姓名" value="<%=data!=null?data.name:"" %>" />
                </p>
                <p>
                    <label>手机：</label>
                    <input type="text" class="form_text" id="txtMobile" placeholder="请输入您的电话" value="<%=data!=null?data.mobile:"" %>"  />
                </p>
                <p>
                    <label>省份：</label>
                    <select class="form_select" id="s_province" name="s_province">
                    </select>
                </p>
                <p>
                    <label>城市：</label>
                    <select class="form_select" id="s_city" name="s_city"></select>
                </p>
                <p>
                    <label>县区：</label>
                    <select class="form_select" id="s_county" name="s_county"></select>
                </p>
            </div>
            <div class="btn1" onclick="btnSubmit()">
                火速预约
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
    s_city = "<%=data!=null?data.city:"城市" %>"
    s_county = "<%=data!=null?data.address:"县区" %>";
    $(function () {
        _init_area();
        $("#s_province").val(s_province).change();
        $("#s_city").val(s_city).change();
        $("#s_county").val(s_county).change();
    });


    function checkForm() {
        if ($("#txtName").val().length == 0) {
            layer.open({
                content: '请输入您的姓名',
                time: 2 //2秒后自动关闭
            });
            return false;
        }
        var tel = $("#txtMobile").val();
        var reg = /^0?1[2|3|4|5|7|8][0-9]\d{8}$/;
        if (!reg.test(tel)) {
            layer.open({
                content: '请输入正确的手机号码',
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
        if ($("#s_city").val().length == 0 || $("#s_city").val() == "城市") {
            layer.open({
                content: '请选择城市',
                time: 2 //2秒后自动关闭
            });
            return false;
        }
        if ($("#s_county").val().length == 0 || $("#s_county").val() == "县区") {
            layer.open({
                content: '请选择县区',
                time: 2 //2秒后自动关闭
            });
            return false;
        }
        return true;
    }

    //朋友助力
    function btnSubmit() {
        if (!checkForm()) return;
        if (isSignUp == "1") {
            layer.open({
                content: "您已经预约过了，无需重复预约！",
                time: 2 //2秒后自动关闭
            });
            return
        }
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
                type: 1
            },
            success: function (ret) {
                loading.close();
                if (ret.code == 1) {
                    window.location.href = "success.html";
                }
                else {
                    layer.open({
                        content: ret.msg,
                        time: 2 //2秒后自动关闭
                    });
                }
            },
            error: function (err) {
                layer.open({
                    content: "很抱歉！您预约失败！",
                    time: 2 //2秒后自动关闭
                });
                loading.close();
            }
        });
    }


</script>
