<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="otherIndex.aspx.cs" Inherits="msite.A1001.otherIndex" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>助力</title>
    <meta name="viewport" content="width=device-width,minimum-scale=1,user-scalable=no,maximum-scale=1,initial-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <script src="/js/jquery-1.7.2.min.js"></script>
    <script src="/js/jquery.qrcode.min.js"></script>    
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
	} /* 滚动条的滑轨背景颜色 */

	::-webkit-scrollbar-thumb {
		  background-color: #590000; 
	} /* 滑块颜色 */

	::-webkit-scrollbar-button {
		  background-color: #590000;
	} /* 滑轨两头的监听按钮颜色 */

	::-webkit-scrollbar-corner {
		  background-color: #590000;
	} /* 横向滚动条和纵向滚动条相交处尖角的颜色 */
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="myCouponCode" Value="" />
        <div>
            <img class="gridlazy" src="/images/main-01_01.jpg" style="width: 100%;" />
        </div>

        <div class="content">
            <div class="content-title2">
                <img src="/images/title.jpg" height="30" />
            </div>
            <div style="margin: 10px; border-bottom: 1px dashed #ffffff; line-height: 35px;">
                <span style="position: relative; left: 0px;">亲友团</span>
                <span style="float: right;">助力指数(元)</span>
            </div>
            <div id="dataList" style="max-height: 350px;overflow: auto;">
                <div class="user_row">
                    <span class="face">
                        <img src="/images/face-none.png" /></span>

                    <span class="face-nick">
                        <span style="display: block; font-size: 20px;">张三</span>
                        <span style="font-size: 12px;">为了朋友，大奖加油哦！</span>
                    </span>
                    <span class="face-right">3
                    </span>
                </div>
                <div class="user_row">
                    <span class="face">
                        <img src="/images/face-none.png" /></span>

                    <span class="face-nick">
                        <span style="display: block; font-size: 20px;">张三</span>
                        <span style="font-size: 12px;">为了朋友，大奖加油哦！</span>
                    </span>
                    <span class="face-right">3
                    </span>
                </div>
                <div class="user_row">
                    <span class="face">
                        <img src="/images/face-none.png" /></span>

                    <span class="face-nick">
                        <span style="display: block; font-size: 20px;">张三</span>
                        <span style="font-size: 12px;">为了朋友，大奖加油哦！</span>
                    </span>
                    <span class="face-right">3
                    </span>
                </div>
                <div class="user_row">
                    <span class="face">
                        <img src="/images/face-none.png" /></span>

                    <span class="face-nick">
                        <span style="display: block; font-size: 20px;">张三</span>
                        <span style="font-size: 12px;">为了朋友，大奖加油哦！</span>
                    </span>
                    <span class="face-right">3
                    </span>
                </div>
                <div class="user_row">
                    <span class="face">
                        <img src="/images/face-none.png" /></span>

                    <span class="face-nick">
                        <span style="display: block; font-size: 20px;">张三</span>
                        <span style="font-size: 12px;">为了朋友，大奖加油哦！</span>
                    </span>
                    <span class="face-right">3
                    </span>
                </div>
            </div>


            <div style="margin: 10px; border-bottom: 1px dashed #ffffff; line-height: 45px; text-align: center; font-weight: bold; font-size: 22px;">
                当前价格是：<span style="color: #F7AA01">0</span>元
            </div>
             <div onclick="myQrcode();" class="btn" style="margin: 10px auto;">我的优惠券</div>
            <div style="margin: 10px; line-height: 35px; text-align: center;">
                “已有0人帮我助力”
            </div>
        </div>

        <div style="margin: 20px;">

            <div onclick="HelpBargain();" style="position: relative; float: left;" class="btn">帮他助力</div>
            <div onclick="myGo();" style="float: right;" class="btn">我要参与</div>

        </div>


        <div style="text-align: center; margin-top: 80px; color: #ffffff;">
            <div>
                <div id="code" style="padding: 5px 5px 0 5px; background-color: #ffffff; width: 205px; height: 205px; margin: 0 auto; background-image: url(/images/def_qrcode.jpg); background-repeat: no-repeat; background-size: cover;">
                </div>
                <div style="font-size: 20px;">
                    现在扫码即刻拿特权
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
    <div id="myqrcode" style="display:none;">
        <div style="margin: 0 auto;">
            <div id="coded" style="width: 250px; height: 250px; padding-top: 150px; margin: 0 auto;">
            </div>
            <div onclick="myCloseQrcode();" class="btn" style="margin: 0 auto; color: #ffffff; margin-top: 20px;">关闭</div>
        </div>
    </div>
</body>
</html>
<script src="/js/loading/jquery.loading-0.1.js"></script>
<link href="/js/layer/need/layer.css" rel="stylesheet" />
<script src="/js/layer/layer.js"></script>
<script type="text/javascript">
    $(function () {
        $("#coded").qrcode({
            render: "table",
            size: 250, //宽度 
            text: "http://www.baidu.com"
        });
    });

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
</script>
