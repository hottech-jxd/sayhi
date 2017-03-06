
/**
*保险列表
*/
define(function (require, exports, module) {

    var layer = require("layer");
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
        url: '/AjaxHandler.aspx?action=UserCarList',//数据来源Url|通过mobel自定义属性配置
        dataParam: {
            type: 1,
            status: -2
        },
        rows: [
            {
                width: '10%', field: 'OrderNo', title: '订单号', align: 'center'
            },
            {
                width: '10%', field: 'carType', title: '车型', align: 'center'
            },
            {
                width: '10%', field: 'invoicePrice', title: '开票价格', align: 'center'
            },
            {
                width: '10%', field: 'InsuranceType', title: '保险险种', align: 'center'
            },
            {
                width: '10%', field: 'FristBeneficiary', title: '第一受益人', align: 'center'
            },
            { width: '6%', field: 'agreement', title: '特别约定', align: 'center' },
            {
                width: '6%', field: 'WIE', title: '立即生效', align: 'center', formatter: function (value, rowData) {
                    return value > 0 ? "是" : "否";
                }
            },
            { width: '6%', field: 'City', title: '所在城市', align: 'center' },
            { width: '18%', field: 'CreateTime', title: '报名时间', align: 'center' },
            {
                width: '6%', field: 'Status', title: '状态', align: 'center', formatter: function (value, rowData) {
                    var text = "申请中";
                    if (value == 1)
                        text = "申请通过";
                    else if (value == -1)
                        text = "申请失败";
                    return text;
                }
            },
            {
                width: '6%', field: 'CustomerId', title: '客户资料', align: 'center', formatter: function (value, rowData) {
                    var html = "";
                    if (rowData["Status"] == 0)
                        html += "<a href='javascript:' class='js-hot-modelCustomer' data-action='update' data-id='" + rowData.id + "' style='margin-right:10px; color:blue;'>审批</a>";
                    html += "<a href='javascript:' class='js-hot-modelCustomer' data-action='get' data-id='" + rowData.CustomerId + "' style='margin-right:10px; color:blue;'>客户资料</a>";
                    return html;
                }
            }
        ]
    }, function () {
        getCustomerInfo();
    });

    //TODO:搜索
    $("#jq-cms-search").click(function () {
        var option = {
            dataParam: {
                type: 1,
                name: $("#modelName").val(),
                status: $("#sltStatus").val()
            }
        };
        ModelGrid.Refresh(option);
    });

    //TODO:显示所有
    $("#jq-cms-searchAll").click(function () {
        var option = {
            dataParam: {
                type: 1,
                name: "",
                status: -2
            }
        };
        ModelGrid.Refresh(option);
    });

    function getCustomerInfo() {
        var obj = $(".js-hot-modelCustomer");
        $.each(obj, function (item, dom) {
            $(dom).click(function () {
                var id = $(this).attr("data-id");//Html5可以使用$(this).data('id')方式来写;                
                var action = $(this).attr("data-action");
                if (action == "get") {
                    layer.open({
                        type: 2,
                        title: '客户资料',
                        shadeClose: true,
                        shade: 0.3,
                        offset: "10px",
                        area: ['550px', 'auto'],
                        btn: ["关闭"],
                        content: 'CustomerInfo.html?id=' + id //iframe的url
                    });
                }
                else if (action == "update") {
                    layer.open({
                        type: 2,
                        title: '申请审批',
                        shadeClose: true,
                        shade: 0.3,
                        offset: "10px",
                        area: ['550px', 'auto'],
                        content: 'CheckResult.html?id=' + id //iframe的url
                    });
                }
            });
        });
    }

});