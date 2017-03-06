//#region 错误提示
jQuery.extend(jQuery.validator.messages, {
    required: "必填项",
    remote: "请修正该字段",
    email: "请输入正确格式的电子邮件",
    url: "请输入合法的网址",
    date: "请输入合法的日期",
    dateISO: "请输入合法的日期 (ISO).",
    number: "请输入合法的数字",
    digits: "只能输入整数",
    creditcard: "请输入合法的银行卡号",
    equalTo: "请再次输入相同的值",
    accept: "请输入拥有合法后缀名的字符串",
    maxlength: jQuery.validator.format("长度最多是 {0} 个字符"),
    minlength: jQuery.validator.format("长度最少是 {0} 个字符"),
    rangelength: jQuery.validator.format("请输入一个长度介于 {0} 和 {1} 之间的字符串"),
    range: jQuery.validator.format("请输入一个介于 {0} 和 {1} 之间的值"),
    max: jQuery.validator.format("请输入一个最大为 {0} 的值"),
    min: jQuery.validator.format("请输入一个最小为 {0} 的值")
});
$.validator.setDefaults({
    errorPlacement: function (error, element) {
        var el = element;
        if (el.parent().hasClass("ui-input-text")) {
            el = el.parent();
        }
        if (el.parent().parent().hasClass("ui-select")) {
            el = el.parent().parent();
        }
        el.after(error);
    }
});
//#endregion
//#region 验证规则
jQuery.validator.addMethod("imparityTo", function (value, element, param) {
    return this.optional(element) || !(value == $(element).parents("form").find(param).val());
}, "请输入不同的值");
jQuery.validator.addMethod("money", function (value, element) {
    var rel = /^\d{1,12}(?:\.\d{1,2})?$/;
    return this.optional(element) || (rel.test(value));
}, "输入的金额格式不对");
 //10位及以下纯数字   
jQuery.validator.addMethod("isNum", function (value, element) {
    var rel = /^\d{1,10}$/;
    return this.optional(element) || !(rel.test(value));
}, "不能使用10位（含）以下纯数字");
jQuery.validator.addMethod("nick", function (value, element) {
    var reg = /^([a-z0-9A-Z_]|[\u4e00-\u9fa5]){2,16}$/;
    return this.optional(element) || (reg.test(value));
}, "请使用中英文、数字,且长度为2至16个字符")
jQuery.validator.addMethod("username", function (value, element) {
    var rel = /^[a-z0-9A-Z_]{3,16}$/;
    return this.optional(element) || (rel.test(value));
}, "请使用字母、数字、下划线，且长度为3至16个字符");
jQuery.validator.addMethod("mopbilephone", function (value, element) {
    var rel = /^1[3|4|5|7|8][0-9]\d{8}$/;
    return this.optional(element) || (rel.test(value));
}, "请输入正确格式的手机号码");
jQuery.validator.addMethod("mopbileorcall", function (value, element) {
    var rel = /^(1[3|4|5|7|8][0-9]\d{8})|(((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?)$/;
    return this.optional(element) || (rel.test(value));
}, "请输入正确格式的手机或电话号码");
jQuery.validator.addMethod("positive", function (value, element) {
    var rel = /^[1-9]\d*$/;
    return this.optional(element) || (rel.test(value));
}, "请输入正整数");
jQuery.validator.addMethod("selrequired", function (value, element, paramvalue) {
    return this.optional(element) || !(value == paramvalue);
}, "请选择项");
jQuery.validator.addMethod("isPrice", function (value, element) {
    var rel = /^(0|[1-9]\d*)(\.\d{1,2})?$/;
    return this.optional(element) || (rel.test(value));
}, "请输入正确的价格");
jQuery.validator.addMethod("idcard", function (value, element) {
    var rel = /^((\d{15})|(\d{17}(\d|X|x)))$/;
    return this.optional(element) || (rel.test(value));
}, "请输入正确的身份证号码");
