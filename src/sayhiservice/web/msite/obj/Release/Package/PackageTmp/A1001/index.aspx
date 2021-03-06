﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="msite.A1001.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>友邦首届名师设计节开幕啦！点点点，千元现金免费领！</title>
    <meta name="viewport" content="width=device-width,minimum-scale=1,user-scalable=no,maximum-scale=1,initial-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
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
            margin: 10px 10px 0px 10px;
            height: auto;
            color: #ffffff;
            border: 1px dashed #ffffff;
        }



            .content ul li span {
                color: #ffffff;
                font-family: cursive;
            }

            .content ul {
                list-style-type: square;
                color: #F7AA01;
                line-height: 25px;
            }

                .content ul li .yellow {
                    color: #F7AA01;
                }


        .content-title2 {
            position: relative;
            width: 150px;
            top: -16px;
            margin: 0 auto;
            line-height: 30px;
            border-radius: 10px;
            text-align: center;
            color: #590000;
            font-weight: bold;
            background-color: #ffffff;
        }
         .btn {
            width: 95%;
            text-align: center;
            line-height: 45px;
            border-radius: 5px;
            background-color: #F7AA01;
            font-weight: bold;
            font-size: 25px;
            margin:0px 10px 0px 10px;
            color: #ffffff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    </form>
    <div>
        <img src="/images/main-01_01.jpg" style="width: 100%;" />
    </div>
    <div>
        <img src="/images/main02.jpg" style="width: 100%;" />
        <img src="/images/main.jpg" style="width: 100%;" onclick="myIndex();" />
    </div>
    <div>
        <img src="/images/title1.jpg" style="width: 100%;" />
    </div>
    <div class="content-title">
        <span>活动期间，在友邦微信活动页面，呼朋唤友来点“助力”，可免费获取超大价值优惠券。“助力”好友越多，优惠券价值越高，最高1000元。</span>
    </div>
    <div class="content">
        <div class="content-title2">
            特权券使用规则
        </div>
        <ul>
            <li><span>此券领取后自动存于微信活动页，消费结账时出示特权券二维码，若未及时出示则当次不得享受优惠。</span></li>
            <li><span>友邦全部产品通用(爆款、特价产品除外)，满<span class="yellow">5000元</span>可用，最多可抵用<span class="yellow">500元</span>；满<span class="yellow">10000元</span>最多可抵用<span class="yellow">1000元</span>。</span></li>
            <li><span>仅限线下门店活动使用，每户仅限一张。</span></li>
            <li><span>此券截至2016年5月2日有效，逾期无效。</span></li>
            <li><span>此券用于抵扣消费金额，不兑现、不找零。</span></li>
            <li><span>导购员将对此券进行核销。</span></li>
        </ul>
    </div>
    <div style="margin-top: 20px;">
        <img src="/images/title2.jpg" style="width: 100%;" />
    </div>
    <div class="content-title">
        <span>活动期间，只要登录友邦微信活动页面领券，就可享受微信上被抽大奖的机会。</span>
    </div>


    <div class="content">
        <div class="content-title2">
            抽奖规则
        </div>
        <ul>
            <li><span>中奖名单将由系统在领券的客户中随机抽取。</span></li>
            <li><span>抽奖时间: <span class="yellow">2016年4月17、24以及5月2日，下午16：00</span>全国同步抽奖。</span></li>
            <li><span>中奖信息查询: 抽奖结束后，于当天晚上<span class="yellow">20：00</span>在友邦集成吊顶服务号统一进行公布。</span></li>
            <li><span>一等奖15名: 奖品为价值<span class="yellow">1998元</span>的高端智能晾衣架一台。</span></li>
            <li><span>二等奖100名: 奖品为价值<span class="yellow">688元</span>的高端欧洲品牌BergHOFF锅一只。</span></li>
            <li><span>三等奖500名: 奖品为价值<span class="yellow">288元</span>的高级健康羽绒枕一个。</span></li>
        </ul>
    </div>
        <div onclick="goFree();" class="btn" style="margin: 10px auto;">抢0元设计</div>
    <div class="footer">
        活动时间：即日起，截至2016年5月2日
    </div>
</body>
</html>
<script type="text/javascript">
    var shareOpenId = '<%=shareOpenId%>'
    function myIndex() {
        window.location.href = 'myIndex.aspx?shareopenid=' + shareOpenId;
    }

    function goFree() {
        window.location.href = "/A1002/freeIndex.aspx";
    }
</script>
