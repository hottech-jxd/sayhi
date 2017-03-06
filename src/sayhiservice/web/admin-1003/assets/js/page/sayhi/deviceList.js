/**
 * Created by Administrator jxd 2015/12/21.
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
        url: '/AjaxHandler.aspx?action=devicelist',//数据来源Url|通过mobel自定义属性配置
        rows: [
             {
                 width: '5%', field: 'deviceid', title: 'deviceid', align: 'center'
             },
            {
                width: '15%', field: 'devicename', title: '设备名称', align: 'center'
            },
            {
                width: '15%', field: 'deviceno', title: '设备号', align: 'center'
            },
            {
                width: '15%', field: 'osversion', title: '系统版本', align: 'center'
            },
            {
                width: '15%', field: 'brand', title: '品牌', align: 'center'
            },
            {
                width: '20%', field: 'remark', title: '备注说明', align: 'center'
            },
             {
                 width: '10%', field: 'createtime', title: '创建时间', align: 'center'
             },
            {
                width: '15%', field: '', title: '操作', align: 'center', formatter: function (value, row) {
                    var opt = "<a style='cursor:pointer;margin-left:20px' data-id='" + row["deviceid"] + "' class='js-hot-modelSee' href='editdevice.html?deviceid="+ row["deviceid"]+"'>查看</a>";
                    opt += "<a style='cursor:pointer;margin-left:20px' data-id='" + row["deviceid"] + "' class='js-hot-modelDelete'>删除</a>";
                   
                    return opt;
                }
            }

        ]
    }, function () {
        deleteModel();
    });

    //TODO:搜索
    $("#jq-cms-search").click(function () {
        var option = {
            dataParam: {
                name: $("#modelName").val(),
                area: $("#s_county").val() != "县区" ? $("#s_county").val() : "",
                city: $("#s_city").val() != "城市" ? $("#s_city").val() : "",
                pro: $("#s_province").val() != "省份" ? $("#s_province").val() : ""
            }
        };
        ModelGrid.Refresh(option);
    })

    //TODO:显示所有
    $("#jq-cms-searchAll").click(function () {
        var option = {
            dataParam: {
                name: "",
                area: "",
                city: "",
                pro: ""
            }
        };
        ModelGrid.Refresh(option);
    })


    function deleteModel() {
        var obj = $(".js-hot-modelDelete");
        $.each(obj, function (item, dom) {
            $(dom).click(function () {//绑定删除事件
                var deviceid = $(this).attr("data-id");//Html5可以使用$(this).data('id')方式来写;
     
                var layer = require("layer");
                layer.confirm('您确定要删除该设备记录吗？', {
                    btn: ['确定', '取消'] //按钮
                }, function () {
                    $.ajax({
                        url: "/AjaxHandler.aspx?Action=deletedevice",
                        data: {
                            deviceid: deviceid
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
                                        layer.msg( data.msg , { time: 2000 });
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