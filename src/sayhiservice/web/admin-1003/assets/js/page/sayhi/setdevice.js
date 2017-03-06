define(function (require, exports, module) {
    var commonUtil = require("common");

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
        url: '/AjaxHandler.aspx?action=devicelist',//数据来源Url|通过mobel自定义属性配置
        rows: [
             {
                 width: '0%', field: 'deviceid', title: '', align: 'center'
             },
            {
                width: '20%', field: 'devicename', title: '设备名称', align: 'center'
            },
            {
                width: '20%', field: 'deviceno', title: '设备识别码', align: 'center'
            },
            {
                width: '10%', field: 'osversion', title: '系统版本', align: 'center'
            },
            {
                width: '15%', field: 'brand', title: '品牌', align: 'center'
            },
              {
                  width: '25%', field: 'remark', title: '备注', align: 'center'
              },
            {
                width: '10%', field: '', title: '操作', align: 'center', formatter: function (value, row) {
                    var opt = "";// "<a href='javascript:alert(1);'>修改</a>";
                    opt += "<a style='cursor:pointer;margin-left:20px' data-id='" + row["deviceid"] + "' class='js-hot-modelSelect'>选择</a>";
                    return opt;
                }
            }
        ]
    }, function () {
        selectModel();
    });



    function selectModel() {
        var obj = $(".js-hot-modelSelect");
        $.each(obj, function (item, dom) {
            $(dom).click(function () {//绑定事件
                var deviceid = $(this).attr("data-id");//Html5可以使用$(this).data('id')方式来写;

                var taskid = commonUtil.getQuery("taskid");
                //alert(taskid);

                var indexdddd = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                
                var layer = require("layer");
                layer.confirm('您确定要设置该设备任务吗？', {
                    btn: ['确定', '取消'] //按钮
                }, function (index) {
                    $.ajax({
                        url: "/AjaxHandler.aspx?Action=settaskdevice",
                        data: {
                            deviceid: deviceid,
                            taskid:taskid
                        },
                        type: "POST",
                        dataType: 'json',
                        success: function (data) {
                            if (data != null) {
                                var code = parseInt(data.code);
                                switch (code) {
                                    case 200:
                                        layer.msg("设置成功", { time: 2000 });
                                        layer.close(index);
                                        //console.log(parent.location);
                                        parent.location.reload();
                                        parent.layer.close(indexdddd); //再执行关闭    
                                    
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
                });
            })
        })
    }


});