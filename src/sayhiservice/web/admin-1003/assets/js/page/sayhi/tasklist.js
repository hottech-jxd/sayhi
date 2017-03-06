/**
 * Created by Administrator xhl 2015/12/21.
 */
define(function (require, exports, module) {

    //TODO:初始化加载模型列表
    var ModelGrid = $("#js-ModelList").Grid({
        method: 'POST',//提交方式GET|POST
        pageSize: 10,
        height: 'auto',
        showNumber: false,
        pageSize: 20,
        pagerCount: 10,
        doubleLine: true,
        pageDetail: true,
        url: '/AjaxHandler.aspx?action=tasklist',//数据来源Url|通过mobel自定义属性配置
        rows: [
             {
                 width: '5%', field: 'taskid', title: 'taskid', align: 'center'
             },
             {
                 width:'15%',field:'remark',title:'任务描述',align:'center'
             },
            {
                width: '10%', field: 'starttime', title: '开始时间', align: 'center'
            },
            {
                width: '10%', field: 'stoptime', title: '结束时间', align: 'center'
            },
            {
                width: '20%', field: 'sayhi', title: '打招呼内容', align: 'center'
            },
            {
                width: '5%', field: 'status', title: '状态', align: 'center', formatter: function (value, row) {
                    if (value == 0)
                        return "未开始";
                    else if( value == 1)
                        return "<font color='green' style='font-weight:bold' >运行中</font>";
                    else if (value == 8) {
                        return "<font color='red' >已完成</font>";
                    }
                }
            },
              {
                  width: '10%', field: 'deviceno', title: '设备号', align: 'center'
              },
            {
                width: '10%', field: 'createtime', title: '创建时间', align: 'center'
            },
            {
                width: '10%', field: '', title: '操作', align: 'center', formatter: function (value, row) {                   
                    var opt = "<a data-id='" + row["taskid"] + "' href='EditTask.html?taskid=" + row["taskid"] + "'>修改</a>";                 
                    if (row["deviceno"].length < 1 || typeof (row["deviceno"]) == "undefined") {
                        opt += "<a style='cursor:pointer;margin-left:20px' data-id='" + row["taskid"] + "' class='js-hot-modelSetDevice'>设置设备</a>";
                    } else {
                        opt += "<a style='cursor:pointer;margin-left:20px' data-id='" + row["taskid"] + "' class='js-hot-modelClearDevice'>移除设备</a>";
                    }

                    opt += "<a style='cursor:pointer;margin-left:20px' data-id='" + row["taskid"] + "' class='js-hot-modelDelete'>删除</a>";
                    return opt;
                }
            }

        ]
    }, function () {
        deleteModel();

        setDevice();

        clearDevice();

    });


    //TODO:搜索
    $("#jq-cms-search").click(function () {
        var option = {
            dataParam: {
                starttime: $("#starttime").val(),
                deviceno: $("#deviceno").val(),
                status: $("#status").val(),               
            }
        };
        ModelGrid.Refresh(option);
    })



    function deleteModel() {
        var obj = $(".js-hot-modelDelete");
        $.each(obj, function (item, dom) {
            $(dom).click(function () {//绑定删除事件
                var taskid = $(this).attr("data-id");//Html5可以使用$(this).data('id')方式来写;
                //alert(taskid);
                var layer = require("layer");
                layer.confirm('您确定要删除该条任务数据吗？', {
                    btn: ['确定', '取消'] //按钮
                }, function () {
                    $.ajax({
                        url: "/AjaxHandler.aspx?action=deletetask",
                        data: {
                            taskid: taskid
                        },
                        type: "POST",
                        dataType: 'json',
                        success: function (data) {
                            if (data != null) {
                                var code = parseInt(data.code);
                                switch (code) {
                                    case 200:
                                        layer.msg("删除成功", { time: 2000 });
                                        ModelGrid.reLoad();
                                        break;
                                    case 500:
                                        layer.msg(data.message, { time: 2000 });
                                        break;
                                    default:
                                        layer.msg("系统繁忙,请稍后再试...", { time: 2000 });
                                        break;
                                }
                            }
                        }
                    });
                });
            })
        })
    }


    function setDevice() {
        var obj = $(".js-hot-modelSetDevice");
        $.each(obj, function (item, dom) {
            $(dom).click(function () {//绑定设置事件
                var taskid = $(this).attr("data-id");
                var layer = require("layer");
                layer.open({
                    type: 2,
                    title: '设置设备',
                    area: ['600px', '400px'],
                    shade: 0,
                    content: '../../../view/setdevice.html?taskid='+taskid,
                    btn: ['关闭'],
                    yes: function () {
                        layer.closeAll();
                    }                   
                });

            })


        });

    }

    function clearDevice() {
        var obj = $(".js-hot-modelClearDevice");
        $.each(obj, function (item, dom) {
            $(dom).click(function () {//绑定删除事件
                var taskid = $(this).attr("data-id");//Html5可以使用$(this).data('id')方式来写;
                //alert(taskid);
                var layer = require("layer");
                layer.confirm('您确定要移除该条任务的设备吗？', {
                    btn: ['确定', '取消'] //按钮
                }, function () {
                    $.ajax({
                        url: "/AjaxHandler.aspx?action=cleartaskdevice",
                        data: {
                            taskid: taskid
                        },
                        type: "POST",
                        dataType: 'json',
                        success: function (data) {
                            if (data != null) {
                                var code = parseInt(data.code);
                                switch (code) {
                                    case 200:
                                        layer.msg("移除设备成功", { time: 2000 });
                                        ModelGrid.reLoad();
                                        break;
                                    case 500:
                                        layer.msg(data.message, { time: 2000 });
                                        break;
                                    default:
                                        layer.msg("系统繁忙,请稍后再试...", { time: 2000 });
                                        break;
                                }
                            }
                        }
                    });
                });
            })
        })

    }

});