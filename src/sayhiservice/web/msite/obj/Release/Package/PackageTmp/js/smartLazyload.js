/**
 * 功能:图片惰性加载，图片宽高未定下优化。
 * @author voidarea
 * @version 1.0.0
 * @dependencies imgReady
 * smartLayzload.init({class:'gridlazy'});
 */
var smartLazyload = {};
smartLazyload.init = function (options) {
    //默认的列表查询对象
    var defoptions = {
        class: 'gridlazy',
        srcStore: '_src',
        onerrorImgUrl: '/images/grey.png',
        sensitivity: 50
    };
    var _options = $.extend({}, defoptions, options);
    //预加载图片准备
    var _list = [];
    var $list = $('.' + _options.class);
    $list.each(function () {
        _list.push({ width: 0, height: 0, src: this.getAttribute(_options.srcStore), objImg: this });
    });
    myLoadImg(_list, 'src', function () {
        //计算每个图片的高度,并设置
        for (var i = 0; i < _list.length; i++) {
            var _item = _list[i];
            if (_item.width == 0 || _item.height == 0) {
                _item.width = _item.height = 300;
            }
            var _objImg = $(_item.objImg);
            var fitedHeight = parseInt(_objImg.width() / _item.width * _item.height);
            _objImg.height(fitedHeight);
        }
        //执行延迟加载
        lazyLoad.init($list, _options);
    });
};

/**
 * 功能:图片惰性加载。
 * @author haunghm
 * @version 1.0.0
 * @dependencies jquery 或者 zepto
 */
var lazyLoad = {
    init: function (_imglist, options) {
        var defoptions = {
            class: 'gridlazy',
            srcStore: '_src',
            onerrorImgUrl: '/images/grey.png',
            sensitivity: 50
        };
        var _options = $.extend({}, defoptions, options);

        var that = this;
        that.imglist = _imglist;

        that.onerrorImgUrl = _options.onerrorImgUrl;//图片加载失败用什么图片替换
        that.srcStore = _options.srcStore;   //图片真实地址存放的自定义属性
        that.class = _options.class;      //惰性加载的图片需要添加的class
        that.sensitivity = _options.sensitivity;           //该值越小，惰性越强（加载越少）      

        minScroll = 5,
		slowScrollTime = 200,
		ios = navigator.appVersion.match(/(iPhone\sOS)\s([\d_]+)/),
		isIos = ios && !0 || !1,
		isoVersion = isIos && ios[2].split("_");

        isoVersion = isoVersion && parseFloat(isoVersion.length > 1 ? isoVersion.splice(0, 2).join(".") : isoVersion[0], 10),
		isIos = that.isPhone = isIos && isoVersion < 6;

        if (isIos) {

            var startSyAndTime,
			setTimeOut;
            $(window).on("touchstart", function () {
                startSyAndTime = {
                    sy: window.scrollY,
                    time: Date.now()
                },
				setTimeOut && clearTimeout(setTimeOut)
            }).on("touchend", function (e) {
                if (e && e.changedTouches) {
                    var subtractionY = Math.abs(window.scrollY - startSyAndTime.sy);
                    if (subtractionY > minScroll) {
                        var subtractionTime = Date.now() - startSyAndTime.time;
                        setTimeOut = setTimeout(function () {
                            that.changeimg(),
							startSyAndTime = {},
							clearTimeout(setTimeOut),
							setTimeOut = null
                        },
						subtractionTime > slowScrollTime ? 0 : 200)
                    }
                } else {
                    that.changeimg();
                }
            }).on("touchcancel", function () {
                setTimeOut && clearTimeout(setTimeOut),
				startSyAndTime = {}
            })
        } else {
            $(window).on("scroll", function () {
                that.changeimg();
            });
        }
        setTimeout(function () {
            that.trigger();
        }, 90);
    },
    trigger: function () {
        var that = this;
        eventType = that.isPhone && "touchend" || "scroll";
        $(window).trigger(eventType);
    },
    changeimg: function () {
        function loadYesOrno(img) {
            var windowPageYOffset = window.pageYOffset,
			windowPageYOffsetAddHeight = windowPageYOffset + window.innerHeight,
			imgOffsetTop = img.offset().top;
            return imgOffsetTop >= windowPageYOffset && imgOffsetTop - that.sensitivity <= windowPageYOffsetAddHeight;
        }
        function loadImg(img, index) {
            setTimeout(function () {
                var imgUrl = img.attr(that.srcStore);
                img.attr("src", imgUrl);
                img[0].onload || (img[0].onload = function () {
                    $(this).removeClass(that.class).removeAttr(that.srcStore),
				    that.imglist[index] = null,
				    this.onerror = this.onload = null
                },
			    img[0].onerror = function () {
			        this.src = that.onerrorImgUrl,
				    $(this).removeClass(that.class).removeAttr(that.srcStore),
				    that.imglist[index] = null,
				    this.onerror = this.onload = null
			    })
            }, 150);
        }
        var that = this;

        that.imglist.each(function (index, val) {
            if (!val) return;
            var img = $(val);
            if (!loadYesOrno(img)) return;
            if (!img.attr(that.srcStore)) return;
            loadImg(img, index);
        })

    }
};
