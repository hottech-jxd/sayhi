<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="result.aspx.cs" Inherits="msite.A1001.result" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1,user-scalable=no,maximum-scale=1,initial-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <title>核销结果</title>
    <style type="text/css">
        html, body {
            background-color: #590000;
            margin: 0px;
        }

        .btn {
            width: 95%;
            text-align: center;
            line-height: 45px;
            border-radius: 5px;
            background-color: #F7AA01;
            font-weight: bold;
            font-size: 25px;
            margin: 10px 10px 0px 10px;
            color: #ffffff;
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin: 50px auto; text-align: center;">
            <img src="<%=isSuccess?"/images/success.png":"/images/icon_fail.png" %>" width="150" />
        </div>
        <div style="color: #ffffff; font-weight: bold; font-size: 25px; line-height: 30px; text-align: center;">
            <%=showText%>
        </div>

        <div style="margin: 10px; line-height: 45px; text-align: center; font-weight: bold; font-size: 22px;color:#ffffff">
            抵金券价值：<span style="color: #F7AA01"><%=userModel!=null?userModel.money:0 %></span>元
        </div>

        <%if (!isSuccess)
          { %>
        <div onclick="goBind();" class="btn">立即绑定</div>
        <%} %>
    </form>
</body>
</html>
<script type="text/javascript">
    function goBind() {
        window.location.href = "login.aspx";
    }
</script>
