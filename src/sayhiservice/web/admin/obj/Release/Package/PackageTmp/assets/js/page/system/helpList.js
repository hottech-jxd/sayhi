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
        url: '/AjaxHandler.aspx?action=helpUserList',//数据来源Url|通过mobel自定义属性配置
        rows: [
             {
                 width: '5%', field: 'rowIndex', title: '序号', align: 'center'
             },
            {
                width: '5%', field: 'headimgurl', title: '头像', align: 'center', formatter: function (value,rowData) {
                    return '<img src="' + value + '" width="40" height="40"  style="margin:5px" />'
                }
            },
            { width: '15%', field: 'nickname', title: '用户昵称', align: 'center' },
            { width: '15%', field: 'City', title: '地区', align: 'center' },
        ]
    }, function () {
    });

    //TODO:搜索
    $("#jq-cms-search").click(function () {
        var option = {
            dataParam: {
                name: $("#modelName").val()
            }
        };
        ModelGrid.Refresh(option);
    })

    //TODO:显示所有
    $("#jq-cms-searchAll").click(function () {
        var option = {
            dataParam: {
                name: ""
            }
        };
        ModelGrid.Refresh(option);
    })

});