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
        url: '/AjaxHandler.aspx?action=shopList',//数据来源Url|通过mobel自定义属性配置
        rows: [
             {
                 width: '5%', field: 'rowIndex', title: '序号', align: 'center'
             },
            {
                width: '10%', field: 'loginName', title: '用户名', align: 'center'
            },
            {
                width: '10%', field: 'area', title: '大区', align: 'center'
            },
            { width: '6%', field: 'pro', title: '省份', align: 'center' },
            { width: '6%', field: 'city', title: '区域', align: 'center' },
            { width: '20%', field: 'shopName', title: '店名', align: 'center' },
            { width: '10%', field: 'name', title: '经销商', align: 'center' },
            { width: '25%', field: 'shopAddress', title: '地址', align: 'center' },
            {
                width: '10%', field: '', title: '操作', align: 'center', formatter: function (value,rowData) {
                    return "<a href='modelList.html?shopid="+rowData.id+"'>核销记录</a>";
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
                area: $("#selarea").val(),
                city: "",
                pro: $("#selpro").val()
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
                pro:""
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