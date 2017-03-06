/**
 * Created by Administrator jxd 2015/12/21.
 */
define(function (require, exports, module) {

    var commonUtils = require("common");

    initDevice();


    function initDevice() {
        var layer = require("layer");
        var id = commonUtils.getQuery("deviceid");
        if (typeof (id) == "undefined") {
            layer.msg("参数错误");
            return;
        }
        if (id.length < 1) {
            layer.msg("参数错误");
            return;
        }

        var deviceid = $("#deviceid");
        var devicename = $("#devicename");
        var deviceno = $("#deviceno");
        var osversion = $("#osversion");
        var brand = $("#brand");
        var createtime = $("#createtime");
        var remark = $("#remark");

        $.ajax({
            url: "/AjaxHandler.aspx?Action=getdevice",
            data: {
                deviceid: id
            },
            type: "POST",
            dataType: 'json',
            success: function (data) {
                if (data != null) {
                    var code = parseInt(data.code);
                    switch (code) {
                        case 200:

                            //console.log(data);
                            //layer.msg("删除成功", { time: 2000 });
                            //ModelGrid.reLoad();
                            deviceid.val(data.data.deviceid);
                            devicename.val(data.data.devicename);
                            deviceno.val(data.data.deviceno);
                            osversion.val(data.data.osversion);
                            createtime.val(data.data.createtime);
                            brand.val(data.data.brand);
                            remark.val(data.data.remark);

                            break;
                        case 500:
                            layer.msg(data.msg, { time: 2000 });
                            break;
                        default:
                            layer.msg("系统繁忙,请稍后再试...", { time: 2000 });
                            break;
                    }
                }
            }
        });


    }


    $("#savedevice").click(function () {

        var remark = $("#remark").val();
        var id = $("#deviceid").val();
        var layer = require("layer");

        $.ajax({
            url: "/AjaxHandler.aspx?Action=updatedevice",
            data: {
                deviceid: id,
                remark:remark,
            },
            type: "POST",
            dataType: 'json',
            success: function (data) {
                if (data != null) {
                    var code = parseInt(data.code);
                    switch (code) {
                        case 200:
                            layer.msg(data.message, { time: 2000 });
                            break;
                        case 500:
                            layer.msg(data.message, {time:2000});
                            break;
                        default:
                            layer.msg("系统繁忙,请稍后再试...", { time: 2000 });
                            break;
                    }
                }
            }
        })

    })


});