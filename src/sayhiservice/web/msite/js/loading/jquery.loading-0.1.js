
/// <reference path="jquery-1.8.3.min.js" />

var loading = loading || {};
var tmpl = '    <div class="swt-messageBox-window">' +
  '    <div class="swt-messageBox-title"><span class="swt-messageBox-title2"></span><span class="swt-messageBox-close"></span>' +
   '    </div>' +
  '    <div class="swt-messageBox-content">' +
    '    <div class="swt-messageBox-content-html"></div>' +
  '    <div class="swt-messageBox-buttonset"></div>' +
  '    </div>' +
'    </div>';
var getScriptLocation = function () {
    scriptLocation = "";
    var isOL = new RegExp("(^|(.*?\\/))(jquery.loading-.*?.js)(\\?|$)");
    var scripts = document.getElementsByTagName('script');
    for (var i = 0, len = scripts.length; i < len; i++) {
        var src = scripts[i].getAttribute('src');
        if (src) {
            var match = src.match(isOL);
            if (match) {
                scriptLocation = match[1];
                break;
            }
        }
    }
    return scriptLocation;
}
document.writeln("<link href=\"" + getScriptLocation() + "css/swtMessageBox.css\" rel=\"stylesheet\" type=\"text/css\" />");
var tmplWaitting = '<div id="swt-messageBox-waiting">' +
                        '<div class="bk">' +
                        '</div>' +
                        '<div class="cont">' +
                            '<img src="' + getScriptLocation() + 'css/loading.gif" alt="loading..." /><span class="swt-messageBox-waittingText" style="overflow:hidden; word-break:break-all;">正在加载...</span></div>' +
                    '</div>';
var _show = function (config) {
    var messageBox = $(tmpl).appendTo(document.body).css('z-index', 9999).hide();
    $('<div class="swt-messageBox-overlay"/>').appendTo(document.body).css('z-index', 9998).show();
    messageBox.find('div.swt-messageBox-content-html').html(config.content);
    var waittingBox = $(tmplWaitting).appendTo(document.body).css('z-index', 9999).hide();
    waittingBox.find('span.swt-messageBox-waittingText').html(config.content);
    if (waittingBox) {
        waittingBox.show();
    }
};

$.extend(loading, {
    show: function (title) {
        var config = {};
        config.content = title || '请等待';
        config.type = 'waiting';
        _show(config);
    },
    close: function () {
        $('#swt-messageBox-waiting').remove();
        $('.swt-messageBox-overlay').remove();
        $('.swt-messageBox-window').remove();
        return;
    }
});