
var wxShare = {
    shareData: {},
    InitShare: function (shareData) {
        this.shareData = shareData;
    },
    appendGuideUserId: function (gduid) {
        var _linkurl = window.location.href;
        var _params = [];
        if (_linkurl.indexOf("?") != -1) {
            var str = _linkurl.substr(1);
            _params = str.split("&");
        }

        var flgExsit = false;
        for (var i = 0; i < _params.length; i++) {
            if (_params[i].indexOf('gduid=') != -1) {
                _linkurl.replace(_params[i], 'gduid=' + gduid);
                flgExsit = true;
                break;
            }
        }

        if (!flgExsit) {
            _linkurl += '&gduid=' + gduid;
        }
        return _linkurl;
    }
}
wx.ready(function () {
    wx.onMenuShareAppMessage({//监听“分享给朋友”，按钮点击、自定义分享内容及分享结果接口
        title: wxShare.shareData.title,
        desc: wxShare.shareData.desc,
        link: wxShare.shareData.link,
        imgUrl: wxShare.shareData.img_url,
        trigger: function (res) {
            //alert('用户点击发送给朋友');
        },
        success: function (res) {
            //alert('已分享');
        },
        cancel: function (res) {
            //alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });

    wx.onMenuShareTimeline({//监听“分享到朋友圈”按钮点击、自定义分享内容及分享结果接口
        title: wxShare.shareData.title,
        link: wxShare.shareData.link,
        imgUrl: wxShare.shareData.img_url,
        trigger: function (res) {
           // alert('用户点击分享到朋友圈');
        },
        success: function (res) {
            //alert('已分享');
        },
        cancel: function (res) {
           // alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });

    wx.onMenuShareQQ({//监听“分享到QQ”按钮点击、自定义分享内容及分享结果接口
        title: wxShare.shareData.title,
        desc: wxShare.shareData.desc,
        link: wxShare.shareData.link,
        imgUrl: wxShare.shareData.img_url,
        trigger: function (res) {
            //alert('用户点击分享到QQ');
        },
        complete: function (res) {
            //alert(JSON.stringify(res));
        },
        success: function (res) {
            //alert('已分享');
        },
        cancel: function (res) {
            //alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });

    wx.onMenuShareWeibo({//监听“分享到微博”按钮点击、自定义分享内容及分享结果接口
        title: wxShare.shareData.title,
        desc: wxShare.shareData.desc,
        link: wxShare.shareData.link,
        imgUrl: wxShare.shareData.img_url,
        trigger: function (res) {
            //alert('用户点击分享到微博');
        },
        complete: function (res) {
            //alert(JSON.stringify(res));
        },
        success: function (res) {
            //alert('已分享');
        },
        cancel: function (res) {
            //alert('已取消');
        },
        fail: function (res) {
            //alert(JSON.stringify(res));
        }
    });
});