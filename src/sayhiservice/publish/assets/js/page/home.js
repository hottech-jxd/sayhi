/**
 * Created by Administrator on 2015/12/21.
 */
define(function (require, exports, module) {
    var commonUtil = require("common");
    commonUtil.calcuHeightToTop("con_left",74);
    commonUtil.calcuWidth("content",$("#con_left").width());
    commonUtil.calcuHeightToTop("con_right",74);
    commonUtil.calcuWidth("con_right",$("#con_left").width());

    //左边显示隐藏
    $("#shrink_span").click(Shrink);

    function Shrink(){
        var win = $(window);
        var height = win.height() - 64 - 10;
        var height_c = height - 29 - 8 - 7;
        var width = 0;
        if ($("#con_left").is(":hidden")) {
            $("#con_left").show();
            $("#shrink_span img").attr("src", "/assets/img/ss.gif");
            $("#shrink_span").css("left", "210px");
            $("#con_right").css("left", "210px");
            width = win.width() - $('#con_left').width();
        }
        else {
            $("#con_left").hide();
            $("#shrink_span img").attr("src", "/assets/img/ss2_03.gif");
            $("#shrink_span").css("left", "0px");
            $("#con_right").css("left", "0px");
            width = win.width();
        }
        $("#content").attr("width", width);
        $("#con_right").height(height).width(width);
    }
});