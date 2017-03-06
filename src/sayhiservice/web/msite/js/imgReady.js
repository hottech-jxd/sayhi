/**
* 图片头数据加载就绪事件
* @参考 	http://www.planeart.cn/?p=1121
* @param	{String}	图片路径
* @param	{Function}	尺寸就绪 (参数1接收width; 参数2接收height)
* @param	{Function}	加载完毕 (可选. 参数1接收width; 参数2接收height)
* @param	{Function}	加载错误 (可选)
*/
var imgReady = (function () {
    var list = [], intervalId = null,
    // 用来执行队列
		tick = function () {
		    var i = 0;
		    for (; i < list.length; i++) {
		        list[i].end ? list.splice(i--, 1) : list[i]();
		    };
		    !list.length && stop();
		},
    // 停止所有定时器队列
		stop = function () {
		    clearInterval(intervalId);
		    intervalId = null;
		};
    return function (url, ready, load, error) {
        var check, width, height, newWidth, newHeight,
				img = new Image();
        if (!url) {
            error && error();
            return;
        }
        img.src = url;
        // 如果图片被缓存，则直接返回缓存数据
        if (img.complete) {
            ready(img.width, img.height);
            load && load(img.width, img.height);
            return;
        };
        // 检测图片大小的改变
        width = img.width;
        height = img.height;
        check = function () {
            newWidth = img.width;
            newHeight = img.height;
            if (newWidth !== width || newHeight !== height ||
                // 如果图片已经在其他地方加载可使用面积检测
					newWidth * newHeight > 1024
				) {
                ready(newWidth, newHeight);
                check.end = true;
            };
        };
        check();
        // 加载错误后的事件
        img.onerror = function () {
            error && error();
            check.end = true;
            img = img.onload = img.onerror = null;
        };
        // 完全加载完毕的事件
        img.onload = function () {
            load && load(img.width, img.height);
            !check.end && check();
            // IE gif动画会循环执行onload，置空onload即可
            img = img.onload = img.onerror = null;
        };
        // 加入队列中定期执行
        if (!check.end) {
            list.push(check);
            // 无论何时只允许出现一个定时器，减少浏览器性能损耗
            if (intervalId === null) intervalId = setInterval(tick, 40);
        };
    };
})();

// 快速获取图片头数据，加载就绪后执行回调函数
function loadImg(jsonData, imgUrlName, callback) {
    var count = 0,
			i = 0,
			intervalId = null,
			data = null,
			imgSrc =
			done = function () {
			    if (count === jsonData.length) {
			        clearInterval(intervalId);
			        callback && callback();
			    }
			};
    for (; i < jsonData.length; i++) {
        data = jsonData[i];
        data.height = parseInt(data.height);
        data.width = parseInt(data.width);

        // 如果已知图片的高度，则跳过
        if (data.height >= 0 && data.width >= 0) {
            ++count;
        } else {
            (function (data) {
                imgReady(data[imgUrlName], function (width, height) {
                    // 图片头数据加载就绪，保存宽高
                    data.width = width;
                    data.height = height;
                    ++count;
                }, null, function () {
                    // 图片加载失败，替换成默认图片
                    data.width = 208;
                    data.height = 240;
                    data.imgSrc = 'images/default.jpg';
                    ++count;
                });
            })(data);
        }
    }
    intervalId = setInterval(done, 40);
}

function myLoadImg(jsonData, imgUrlName, callback) {
    var count = 0,
			i = 0,
			intervalId = null,
            widthSum = 0,
            firstWidth = 0 //第一张图片的宽度
    data = null,
			imgSrc =
			done = function () {
			    if (count === jsonData.length) {
			        clearInterval(intervalId);
			        callback && callback(parseInt(widthSum), parseInt(firstWidth));
			    }
			};
    for (; i < jsonData.length; i++) {
        data = jsonData[i];
        data.height = parseInt(data.height);
        data.width = parseInt(data.width);

        // 如果已知图片的高度，则跳过
        if (data.height > 0 && data.width > 0) {
            ++count;
        } else {
            (function (data) {
                imgReady(data[imgUrlName], function (width, height) {
                    // 图片头数据加载就绪，保存宽高
                    if (count == 0) {
                        firstWidth += width / height * 200;
                    }
                    data.width = width;
                    data.height = height;
                    widthSum += width / height * 200;
                    ++count;
                }, null, function () {
                    // 图片加载失败，替换成默认图片
                    //data.width = 208;
                    //data.height = 240;
                    data.src = '/Mall/images/44444444.jpg';
                    ++count;
                });
            })(data);
        }
    }
    intervalId = setInterval(done, 40);
}
