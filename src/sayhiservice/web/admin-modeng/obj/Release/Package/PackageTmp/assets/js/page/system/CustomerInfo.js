
/**
*/
define(function (require) {
    var commonUtil = require("common");
    var layer = require("layer");
    var data = {
        action: "getCustomerInfo",
        id: commonUtil.getQuery("id")
    };
    var index = layer.load();
    commonUtil.ajaxCall("post", "/AjaxHandler.aspx", data, function (ret) {
        layer.close(index);
        if (ret.code == 1) {
            $("#lblName").text(ret.result.UserName);
            $("#lblMobile").text(ret.result.UserMobile);
            if (ret.result.UserImg != null && ret.result.UserImg != "") {
                var imgs = ret.result.UserImg.split("|");
                var imgContent = "";
                $.each(imgs, function (i, item) {
                    imgContent += "<a href='" + item + "' target=\"_blank\"><img width=\"100\" height=\"100\" src=\"" + item + "\" /></a> ";
                });
                $("#imgContent").html(imgContent);
            }
        }
        else {
            
        }
        var pIdx = parent.layer.getFrameIndex(window.name);
        parent.layer.iframeAuto(pIdx);
    }, function (err) { });
});