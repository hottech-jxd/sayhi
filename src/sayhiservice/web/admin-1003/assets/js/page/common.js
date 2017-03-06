/**
 * Created by Administrator on 2015/12/21.
 */
define(["js/jquery-1.9.1.min"], function () {
    return {
        calcuHeight: function (id) {
            var height = $(document).height();
            $("#" + id).height(height);
        },
        calcuHeightToTop: function (id, topHeight) {
            var height = $(document).height() - topHeight;
            $("#" + id).height(height);
        },
        cacleHeightByIframe: function (id, topHeight) {
            var height = $(window).height() - topHeight;
            $("#" + id).height(height);
        },
        calcuWidth: function (id, leftWidth) {
            var win = $(window);
            var width = win.width() - leftWidth;
            $("#" + id).width(width);
        },
        setDisabled: function (id) {
            var disabledText = $("#" + id).attr("data-submiting-text");
            $("#" + id).addClass("disabled");
            $("#" + id).val(disabledText);
        },
        cancelDisabled: function (id) {
            //setTimeout(function () {
            var abledText = $("#" + id).attr("data-text");
            $("#" + id).removeClass("disabled");
            $("#" + id).val(abledText);
            //}, 2000);
        },
        redirectUrl: function (url) {
            parent.frames["content"].src = url;
            //alert("fff");
            //parent.frames["content"].window.href=url;
        },
        /*
        * @brief 获得页面参数
        * @param 参数名
        * */
        getQuery: function (name) {
            var strHref = window.document.location.href;
            var intPos = strHref.indexOf("?");
            var strRight = strHref.substr(intPos + 1);
            var arrTmp = strRight.split("&");
            for (var i = 0; i < arrTmp.length; i++) {
                var arrTemp = arrTmp[i].split("=");
                if (arrTemp[0].toUpperCase() == name.toUpperCase()) return arrTemp[1];
            }
            if (arguments.length == 1)
                return "";
            if (arguments.length == 2)
                return arguments[1];
        },
        /*
         * @brief ajax请求
         * @param type 请求类型
         * @param data 请求数据
         * @param successCallback 成功回调函数
         * @param errorCallback 失败回调函数
         * */
        ajaxCall: function (type, url, data, successCallback, errorCallback) {
            $.ajax({
                url: url,
                data: data,
                type: type,
                dataType: "json",
                success: function (ret) {
                    if (typeof successCallback == "function") {
                        successCallback(ret);
                    }
                },
                error: function (err) {
                    if (typeof errorCallback == "function") {
                        errorCallback(err);
                    }
                }
            });
        },

        isLogin: function () {
            this.ajaxCall("post", "/AjaxHandler.aspx", { action: "isLogin" }, function (ret) {
                if (ret.code != 1) {
                    parent.window.location.href = "/login.html";
                }
            })
        },


        // 对Date的扩展，将 Date 转化为指定格式的String   
        // 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
        // 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
        // 例子：   
        // (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
        // (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
        dateFormat: function (fmt, date) { //author: meizz   
            var o = {
                "M+": date.getMonth() + 1,                 //月份   
                "d+": date.getDate(),                    //日   
                "h+": date.getHours(),                   //小时   
                "m+": date.getMinutes(),                 //分   
                "s+": date.getSeconds(),                 //秒   
                "q+": Math.floor((date.getMonth() + 3) / 3), //季度   
                "S": date.getMilliseconds()             //毫秒   
            };
            if (/(y+)/.test(fmt))
                fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(fmt))
                    fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return fmt;

        },

    }

});