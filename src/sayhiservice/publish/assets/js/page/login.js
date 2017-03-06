/**
 * Created by Administrator on 2015/12/21.
 */
define(function (require, exports, module) {
    var commonUtil = require("common");



    $("#login_btn").click(function () {


        if ($("#username").val().length == 0) {
            layer.tips('请输入用户名', "#username");
            return;
        }
        if ($("#password").val().length == 0) {
            layer.tips('请输入用户密码', "#password");
            return;
        }
        var index = layer.load(1, {
            shade: [0.5, '#333']
        });
        var data = {
            action: "login",
            loginName: $("#username").val(),
            loginPwd: $("#password").val(),
        };
        commonUtil.ajaxCall("post", "/AjaxHandler.aspx", data, function (ret) {
            layer.close(index);
            if (ret.code == 1) {
                layer.msg('登录成功');
                window.location.href = "main.html";
            }
            else {
                layer.msg('用户名或密码不正确');
            }
        }, function (err) { });
    });



});