/**
 * Created by Administrator xhl 2015/12/21.
 */
define(function (require, exports, module) {
    //_init_area();
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
        url: '/AjaxHandler.aspx?action=UserSignList',//数据来源Url|通过mobel自定义属性配置
        rows: [
             {
                 width: '26%', field: 'openid', title: 'openid', align: 'center'
             },
            {
                width: '10%', field: 'name', title: '姓名', align: 'center'
            },
            {
                width: '10%', field: 'mobile', title: '手机', align: 'center'
            },
            {
                width: '10%', field: 'content1', title: '店名', align: 'center'
            },
            {
                width: '10%', field: 'content3', title: '配送商', align: 'center'
            },
            {
                width: '10%', field: 'Province', title: '省份', align: 'center'
            },
            { width: '6%', field: 'city', title: '城市', align: 'center' },
            { width: '6%', field: 'address', title: '区域', align: 'center' },
            { width: '20%', field: 'content2', title: '备注', align: 'center' },
            { width: '18%', field: 'createtime', title: '时间', align: 'center' }
        ]
    }, function () {

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
});