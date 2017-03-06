
define(function (require) {
    var commonUtil = require("common");
    var layer = require("layer");
    var pIdx = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(pIdx);
    $("#submitCheck").click(function () {

        layer.confirm('您确定要执行此操作？', {
            btn: ['确定', '取消'] //按钮
        }, function (index) {
            layer.close(index);
            submitCheck();
        });

    });


    function submitCheck() {
        var data = {
            action: "updateCheck",
            id: commonUtil.getQuery("id"),
            type: $("#sltStatus").val(),
            remark: encodeURIComponent($("#txtRemark").val())
        };
        var index = layer.load();
        commonUtil.ajaxCall("post", "/AjaxHandler.aspx", data, function (ret) {
            layer.close(index);
            if (ret.code == 1) {
                layer.msg("提交成功");
                setTimeout(function () {
                    parent.window.location.reload();
                    parent.layer.close(pIdx);
                }, 1000);
                

            }
            else {
                layer.msg("提交失败");
            }

        }, function (err) {
            layer.close(index);
            layer.msg("提交失败");
        });
    }

});