/**
 * @brief 修改模型功能js
 * @since 1.0.0
 * @author xhl
 * @time 15/12/28
 */
define(function (require, exports, module) {
    $("#UpdateModelForm").validate({
        rules: {
            txtModelName:{
                required: true,
            },
            txtModelDescription:{
                maxlength:200
            },
            txtModelType: {
                selrequired: "-1"
            },
            txtOrderWeight:{
                digits:true,
            }
        },
        messages: {
            txtModelName:{
              required:"模型名称为必输项"
            },
            txtModelDescription:{
                maxlength:"模型描述不能超过200个字符"
            },
            txtModelType: {
                selrequired: "请选择模型类型"
            },
            txtOrderWeight:{
                digits:"请输入数字",
            }
        },
        submitHandler: function (form, ev) {
            var commonUtil = require("common");
            commonUtil.setDisabled("jq-cms-Save");
            $.ajax({
                url: "/model/saveModel",
                data: {
                    id:$("#hidModelID").val(),
                    name: $("#txtModelName").val(),
                    description: $("#txtModelDescription").val(),
                    type: $("#txtModelType").val(),
                    orderWeight: $("#txtOrderWeight").val()
                },
                type: "POST",
                dataType: 'json',
                success: function (data) {
                    var layer=require("layer");
                    if(data!=null)
                    {
                        var index=parseInt(data.code);
                        if(index==200)
                        {
                            var layer=require("layer");
                            layer.msg("修改成功,2秒后将自动返回列表页面",{time: 2000})
                            commonUtil.cancelDisabled("jq-cms-Save");
                            window.location.href="http://"+window.location.host+"/"+"model/modellist";
                            //commonUtil.redirectUrl("/model/modelList");
                            //$("#txtModelName").val("");
                            //$("#txtModelDescription").val("");
                        }
                        if(index==500)
                            layer.msg("修改失败",{time: 2000})
                    }
                    commonUtil.cancelDisabled("jq-cms-Save");
                },
                error: function () {
                    commonUtil.cancelDisabled("jq-cms-Save");
                }
            });
            return false;
        },
        invalidHandler: function () {
            return true;
        }
    });
});
