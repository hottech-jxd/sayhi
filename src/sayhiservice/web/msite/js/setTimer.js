var interval = 1000, _int;
var cc = document.getElementById('shijian');
function timer() {
    var now = new Date();
    var endDate = new Date(defatultDate);
    var leftTime = endDate.getTime() - now.getTime();
    var intDiff = parseInt(leftTime / 1000);
    var day = 0,
        hour = 0,
        minute = 0,
        second = 0;//时间默认值
    if (intDiff > 0) {
        day = Math.floor(intDiff / (60 * 60 * 24));
        hour = Math.floor(intDiff / (60 * 60)) - (day * 24);
        minute = Math.floor(intDiff / 60) - (day * 24 * 60) - (hour * 60);
        second = Math.floor(intDiff) - (day * 24 * 60 * 60) - (hour * 60 * 60) - (minute * 60);
    }
    if ((day < 0 || hour < 0 || minute < 0 || second < 0) || (day <= 0 && hour <= 0 && minute <= 0 && second <= 0)) {
        cc.innerHTML = '倒计时<b class="wsltime">0</b>天<b class="wsltime">0</b>小时<b class="wsltime">0</b>分<b class="wsltime">0</b>秒</span>';
        window.clearInterval(_int);
    } else {
        cc.innerHTML = '倒计时<b class="wsltime">' + day + '</b>天<b class="wsltime">' + hour + '</b>小时<b class="wsltime">' + minute + '</b>分<b class="wsltime">' + second + '</b>秒';
    }
}
_int = window.setInterval(function () { timer(); }, interval);