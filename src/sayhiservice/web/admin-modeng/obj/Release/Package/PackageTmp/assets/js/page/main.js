/**
 * Created by Administrator on 2015/12/21.
 */
define(function (require, exports, module) {
    var commonUtil = require("common");
    initWindow();

    //左边栏目效果
    var menusObj = $(".js-cms-menus");
    $.each(menusObj, function (item, dom) {
        $(dom).find(".aparent").click(function () {
            if ($(dom).find("ul").hasClass('hidden')) {
                $(dom).removeClass("nav-active")
                $(dom).find("ul").removeClass("hidden");
            }
            else {
                $(dom).addClass("nav-active");
                $(dom).find("ul").addClass("hidden");
            }
        });
    });

    // 浏览改大小
    $(window).bind('resize', function () {
        initWindow();
    });


    //初始化窗口
    function initWindow() {
        commonUtil.calcuHeight("mainpanel");
        commonUtil.calcuHeightToTop("con_right", $(".headerbar").height());
        commonUtil.cacleHeightByIframe("con_left", $("#con_left").offset().top);
        commonUtil.calcuWidth("content", $(".leftpanel").width());
        commonUtil.cacleHeightByIframe("content", $(".headerbar").height());
        commonUtil.calcuWidth("con_right", $(".leftpanel").width());
    }


    commonUtil.isLogin();
});

function openUrl(linkUrl, menuid) {
    $(".childMenus").removeClass("active");
    $("#id_" + menuid).addClass("active");
    $('#content').attr('src', linkUrl);
}