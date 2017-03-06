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
        url: '/AjaxHandler.aspx?action=devicelist',//数据来源Url|通过mobel自定义属性配置
        rows: [
             {
                 width: '10%', field: 'deviceid', title: 'deviceid', align: 'center'
             },
            {
                width: '15%', field: 'devicename', title: '设备名称', align: 'center'
            },
            {
                width: '15%', field: 'deviceno', title: '设备识别码', align: 'center'
            },
            {
                width: '15%', field: 'osversion', title: '系统版本', align: 'center'
            },
            {
                width: '15%', field: 'brand', title: '品牌', align: 'center'
            },
            {
                width: '15%', field: 'createtime', title: '创建时间', align: 'center'
            },
            {
                width:'15%',field:'',title:'操作', align:'center'
            }

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