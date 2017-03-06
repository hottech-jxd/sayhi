/**
 * Created by Administrator xhl 2015/12/21.
 */
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
        dataParam: {
            shopid: commonUtil.getQuery("shopid")
        },
        url: '/AjaxHandler.aspx?action=couponList',//数据来源Url|通过mobel自定义属性配置
        rows: [
             {
                 width: '5%', field: 'rowIndex', title: '序号', align: 'center'
             },
            {
                width: '20%', field: 'couponCode', title: '优惠券码', align: 'center'
            },
            {
                width: '40%', field: 'updateTime', title: '生成时间', align: 'center'
            },
            { width: '15%', field: 'money', title: '价值(元)', align: 'center' },
            {
                width: '15%', field: 'useStatus', title: '状态', align: 'center', formatter: function (value, rowData) {
                    return value == 1 ? "已使用" : "未使用";
                }
            },
            {
                width: '15%', field: 'useTime', title: '使用时间', align: 'center', formatter: function (value, rowData) {
                    if (rowData["useStatus"] == 1)
                        return value;
                    return "--";
                }
            },
            { width: '15%', field: 'nickname', title: '用户昵称', align: 'center' },
            {
                width: '15%', field: 'city', title: '所在地', align: 'center'
            },
            {
                width: '15%', field: '', title: '操作', align: 'center', formatter: function (value, rowData) {
                    return "<a href='myHelpUserList.html?userid=" + rowData.id + "'>查看助力列表</a>";
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
                shopid: commonUtil.getQuery("shopid"),
                type: $("#selUse").val(),
                txtStartTime: $("#txtStartTime").val(),
                txtEndTime: $("#txtEndTime").val(),
            }
        };
        ModelGrid.Refresh(option);
    })

    //TODO:显示所有
    $("#jq-cms-searchAll").click(function () {
        var option = {
            dataParam: {
                name: "",
                shopid: commonUtil.getQuery("shopid")
            }
        };
        ModelGrid.Refresh(option);
    })

    //TODO:删除
    function deleteModel() {
        var obj = $(".js-hot-modelDelete");
        $.each(obj, function (item, dom) {
            $(dom).click(function () {//绑定删除事件
                var id = $(this).attr("data-id");//Html5可以使用$(this).data('id')方式来写;
                var layer = require("layer");
                layer.confirm('您确定要删除该条模型记录吗？', {
                    btn: ['确定', '取消'] //按钮
                }, function () {
                    $.ajax({
                        url: "/model/deleteModel",
                        data: {
                            id: id
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
                                    case 202:
                                        layer.msg("对不起,您没有删除权限", { time: 2000 });
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