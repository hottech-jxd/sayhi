<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myIndex.aspx.cs" Inherits="msite.A1001.myIndex" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>大咖来了 ，好想装啊~ <%=(!_self&&userModel!=null)?(userModel.nickname+"邀请您，帮TA加油"):"点击页面"%>，抢千元装修豪礼</title>
    <meta name="viewport" content="width=device-width,minimum-scale=1,user-scalable=no,maximum-scale=1,initial-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <script src="/js/jquery-1.7.2.min.js"></script>
    <script src="/js/jquery.qrcode.min.js?201603311011"></script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript" src="/JSDK/RegConfig.aspx?customerid=<%=this.CurrentCustomerID %>&debug=0"></script>
    <script src="/js/wxShare.js?v=201604111011"></script>
    <style type="text/css">
        html, body {
            background-color: #590000;
            margin: 0px;
        }

        .header {
            background-color: #ffffff;
        }

        .footer {
            bottom: 100px;
            line-height: 100px;
            font-weight: bold;
            font-size: 12px;
            text-align: center;
            color: #ffffff;
            width: 100%;
            clear: both;
        }


        .content-title {
            margin: 30px 10px;
            line-height: 24px;
            color: #ffffff;
        }

            .content-title span {
                line-height: 30px;
                height: 30px;
                font-weight: bold;
            }

            .content-title .num {
                height: 24px;
                width: 24px;
                line-height: 24px;
                display: inline-block;
                display: inline-block;
                background-color: #F7AA01;
                border-radius: 100px;
                color: #590000;
                text-align: center;
                font-size: 22px;
            }

        .content {
            margin: 30px 10px 0px 10px;
            height: auto;
            color: #ffffff;
            border: 1px dashed #ffffff;
            border-radius: 10px;
        }

            .content ul {
                list-style-type: none;
            }

        .content-title2 {
            position: relative;
            top: -14px;
            margin: 0 auto;
            border-radius: 10px;
            text-align: center;
            color: #590000;
            font-weight: bold;
        }

        .face, .face img {
            height: 50px;
            width: 50px;
            border-radius: 100px;
            display: inline-block;
            background-color: #ffffff;
        }

        .user_row {
            margin: 10px 0 20px 10px;
            position: relative;
        }

        .face-nick {
            position: absolute;
            margin: 5px 0 0 10px;
        }

        .face-right {
            position: absolute;
            right: 0;
            margin: 15px 20px 0 0;
        }

        .btn {
            width: 140px;
            text-align: center;
            line-height: 30px;
            border-radius: 5px;
            background-color: #F7AA01;
            font-weight: bold;
            font-size: 20px;
            color: #590000;
        }

        ::-webkit-scrollbar {
            width: 15px;
        }

        ::-webkit-scrollbar-track {
            background-color: #590000;
        }
        /* 滚动条的滑轨背景颜色 */

        ::-webkit-scrollbar-thumb {
            background-color: #590000;
        }
        /* 滑块颜色 */

        ::-webkit-scrollbar-button {
            background-color: #590000;
        }
        /* 滑轨两头的监听按钮颜色 */

        ::-webkit-scrollbar-corner {
            background-color: #590000;
        }
        /* 横向滚动条和纵向滚动条相交处尖角的颜色 */
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="myCouponCode" Value="" />
        <div>
            <img src="/images/main-01_01.jpg" style="width: 100%;" />
        </div>

        <div class="content">
            <div class="content-title2">
                <img src="/images/title.jpg" height="30" />
            </div>
            <div style="margin: 10px; border-bottom: 1px dashed #ffffff; line-height: 35px;">
                <span style="position: relative; left: 0px;">亲友团</span>
                <span style="float: right;">助力指数(元)</span>
            </div>
            <div id="dataList" style="max-height: 350px; overflow: auto;">
            </div>

            <div id="loadMore" style="display: none; border-top: 1px dashed #ffffff; margin: 10px; line-height: 35px; text-align: center;" onclick="loadHelpUser()">
                点击加载更多
            </div>

            <div style="margin: 10px; border-bottom: 1px dashed #ffffff; line-height: 45px; text-align: center; font-weight: bold; font-size: 22px;">
                当前抵金券价值：<span style="color: #F7AA01"><%=userModel!=null?userModel.money:0 %></span>元
                <%if (_self)
                  {  %>
                <div onclick="myQrcode()" class="btn" style="margin: 10px auto;">我的优惠券</div>
                <%} %>
            </div>
            <div style="margin: 10px; line-height: 35px; text-align: center;">
                “已有<%=attendCount %>人帮<%=_self?"我":"TA"%>助力”
            </div>
        </div>

        <div style="margin: 20px;">
            <%if (_self)
              {  %>
            <div onclick="ShareBargain()" style="position: relative; float: left;" class="btn">邀请朋友</div>
            <div onclick="myGoRule()" style="float: right;" class="btn">游戏规则</div>
            <%} %>
            <% else
              { %>
            <div onclick="HelpBargain()" style="position: relative; float: left;" class="btn">帮TA助力</div>
            <div onclick="myGoRule()" style="float: right;" class="btn">我要参与</div>
            <%} %>
        </div>


        <div style="text-align: center; margin-top: 80px; color: #ffffff;">
            <div>
                <div style="padding: 5px 5px 0 5px; background-color: #ffffff; width: 205px; height: 205px; margin: 0 auto;">
                    <img src="/images/qrcode.jpg" height="200" width="200" />
                </div>
                <div style="font-size: 20px; margin-top: 5px;">
                    <b>关注公众号即有机会参与抽奖</b>
                </div>         
            </div>
        </div>
        <div class="footer">
            活动时间：即日起，截至2016年5月2日
        </div>
    </form>

    <!--邀请分享提示-->
    <div id="mcover" onclick="document.getElementById('mcover').style.display='none';" style="display: none; position: fixed; z-index: 1000; top: 0px; bottom: 0px;">
        <img src="/images/pyq.png" style="height: 100%; width: 100%;" />
    </div>
 
    <div id="myqrcode" style="display: none;">
        <div style="margin: 0 auto;">
            <div id="code" style="width: 250px; height: 250px; padding-top: 150px; margin: 0 auto;">
            </div>
			<div style="margin: 10px; line-height: 45px; text-align: center; font-weight: bold; font-size: 22px;">
                当前抵金券价值：<span style="color: #F7AA01"><%=userModel!=null?userModel.money:0 %></span>元
            </div>
            <div style="text-align: center; font-weight: bold; font-size: 16px;">
                优惠券码：<span style="color: #F7AA01"><%=userModel!=null?userModel.couponCode:"" %></span>
            </div>
            <div onclick="myCloseQrcode()" class="btn" style="margin: 0 auto; color: #ffffff; margin-top: 20px;">关闭</div>
        </div>
    </div>

</body>
</html>
<script src="/js/loading/jquery.loading-0.1.js"></script>
<link href="/js/layer/need/layer.css" rel="stylesheet" />
<script src="/js/layer/layer.js"></script>
<script type="text/javascript">
    //活动结束时间
    var defatultDate = '<%=actEndTime%>';
    var pageIndex = 1;
    function goFree() {
        window.location.href = "/A1002/freeIndex.aspx";
    }
    //朋友助力
    function HelpBargain() {
        loading.show();
        $.ajax({
            type: "post",
            url: "/AjaxHandler.aspx",
            data: {
                action: "helpbargain",
                shareopenid: "<%=shareOpenId%>",
                openid: "<%=currentOpenid%>",
                customerid: "<%=CurrentCustomerID%>"
            },
            success: function (ret) {
                loading.close();
                if (ret.code == 1) {
                    layer.open({
                        btn: ['OK'],
                        content: '您成功帮TA助力' + ret.money + '元',
                        yes: function (index) {
                            pageIndex = 1;
                            $("#dataList").html("");
                            loadHelpUser();
                            layer.close(index);
                        }
                    });
                }
                else{
                	layer.open({
                        btn: ['OK'],
                        content: ret.msg
                    });
                }
            },
            error: function (err) {
                loading.close();
            }
        });
    }

    function myGo() {
        window.location.href = 'myIndex.aspx?shareopenid=<%=currentOpenid %>';
    }

    function myGoRule() {
        window.location.href = 'index.aspx';//rule.html
    }



    function loadHelpUser() {
        loading.show();
        var pageSize = 100;
        $.ajax({
            type: "post",
            url: "/AjaxHandler.aspx",
            data: {
                action: "gethelpuserlist",
                shareopenid: "<%=shareOpenId%>",
                pageIndex: pageIndex,
                pageSize: pageSize,
                customerid: "<%=CurrentCustomerID%>"
            },
            success: function (ret) {
                loading.close();
                if (ret.code == 1) {
                    pageIndex = ret.pageIndex;
                    var html = '';
                    if (ret.count > 0) {
                        $.each(ret.data, function (i, item) {
                            html += '<div class="user_row"><span class="face"><img src="{imgUrl}" /></span><span class="face-nick"><span style="display: block; font-size: 20px;">{nickName}</span><span style="font-size: 12px;">为了朋友，大奖加油哦！</span></span><span class="face-right">{money}</span></div>';
                            html = html.replace("{imgUrl}", item.headimgurl);
                            html = html.replace("{nickName}", item.nickname);
                            html = html.replace("{money}", item.money);
                        });
                        $("#dataList").append(html);
                    }
                    if (ret.count >= pageSize) {
                        $("#loadMore").show();
                        pageIndex = parseInt(pageIndex) + 1;
                    }
                    else {
                        $("#loadMore").hide();
                    }
                }
            },
            error: function (err) {
                loading.close();
            }
        });
    }



    //邀请朋友助力
    function ShareBargain() {
        document.getElementById('mcover').style.display = 'block';
    }

    function myQrcode() {
        //页面层
        layer.open({
            type: 1,
            content: $("#myqrcode").html(),
            anim: true,
            style: 'position:fixed; bottom:0; left:0; width:100%;height:100%;  padding:10px 0; border:none;',
            shadeClose: true,
            shade: true

        });

    }

    function myCloseQrcode() {
        layer.closeAll();
    }


    $(function () {
        $("#code").qrcode({
            render: "table",
            size: 250, //宽度 
            text: "http://" + window.location.host + "/A1001/result.aspx?code=" + $("#myCouponCode").val()
        });
        loadHelpUser();
    });

    ///微信分享userModel.nickname
    wxShare.InitShare({
        title: '大咖来了 ，好想装啊~ <%=userModel!=null?userModel.nickname:""%>邀请您，帮TA加油，抢千元装修豪礼',
        desc: '友邦集成吊顶首届名师设计节开幕啦，呼朋唤友来“助力”，可免费最高1000元优惠券，更有大奖等你拿！',
        link: window.location.href,
        img_url: "http://" + window.location.host + "/images/logo.jpg"
    });
</script>
