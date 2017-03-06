
/**
 * Created by Administrator jxd 2015/12/21.
 */
define(function (require, exports, module) {
    var myGeo;
    var myMap;
    var commonUtil = require("common");

    initBaiduMap();

    initTaskInfo();


    function initBaiduMap() {

        // 百度地图API功能
        myMap = new BMap.Map("allmap");    // 创建Map实例
        myMap.centerAndZoom("杭州", 17);  // 初始化地图,设置中心点坐标和地图级别
        myMap.addControl(new BMap.MapTypeControl());   //添加地图类型控件
        myMap.setCurrentCity("杭州");          // 设置地图显示的城市 此项是必须设置的
        myMap.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放

        // 添加带有定位的导航控件
        var navigationControl = new BMap.NavigationControl({
            // 靠左上角位置
            anchor: BMAP_ANCHOR_TOP_LEFT,
            // LARGE类型
            type: BMAP_NAVIGATION_CONTROL_LARGE,
            // 启用显示定位
            enableGeolocation: true
        });
        myMap.addControl(navigationControl);


        myGeo = new BMap.Geocoder(); //


        var size = new BMap.Size(70,20);
        myMap.addControl(new BMap.CityListControl({
            anchor: BMAP_ANCHOR_TOP_LEFT,
            offset: size,
        }));

        myMap.addEventListener("click", function (e) {
            var point = e.point;
            myGeo.getLocation(point, function (rs) {
                showTipWindow(point , rs )
            });

        });

        var ac = new BMap.Autocomplete({
            "input": "suggestId", "location": myMap
        });//建立一个自动完成的对象

        ac.addEventListener("onhighlight", function (e) {
            var str = "";
            var _value = e.fromitem.value;
            var value = "";
            if (e.fromitem.index > -1) {
                value = _value.province + _value.city + _value.district + _value.street + _value.business;
            }
            str = "FromItem<br />index = " + e.fromitem.index + "<br />value = " + value;

            value = "";
            if (e.toitem.index > -1) {
                _value = e.toitem.value;
                value = _value.province + _value.city + _value.district + _value.street + _value.business;
            }
            str += "<br />ToItem<br />index = " + e.toitem.index + "<br />value = " + value;
            document.getElementById("searchResultPanel").innerHTML = str;

        });

        var myvalue;

        ac.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
            var _value = e.item.value;
            myValue = _value.province + _value.city + _value.district + _value.street + _value.business;
            document.getElementById("searchResultPanel").innerHTML = "onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;

            setPlace();
        });

        function setPlace() {
            //myMap.clearOverlays();    //清除地图上所有覆盖物
            function myFun() {
                var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果
                myMap.centerAndZoom(pp, 18);
                //map.addOverlay(new BMap.Marker(pp));    //添加标注
            }
            var local = new BMap.LocalSearch(myMap, { //智能搜索
                onSearchComplete: myFun
            });
            local.search(myValue);
        }

    }

   


    function showTipWindow( point , rs ) {
        var obj = $("#js-ModelList");

        var address = rs.addressComponents;
        
        var stringAddress = address.province + address.city + address.district + address.street + address.streetNumber;

        var layer = require("layer");
        layer.confirm('您确定要增加这个位置信息[纬度='+ point.lng +",经度="+ point.lat +']'+stringAddress+'吗？', {
            btn: ['确定', '取消'] //按钮
        }, function (index) {            
            layer.close(index);
            var _p = "<p style='height:20px;cursor:pointer;' class='js-hot-modelrecord'  data-point-lat=" + point.lat + " data-point-lng=" + point.lng + " data-point-add=" + stringAddress + ">" + stringAddress + " <a class='js-hot-modelDelete' >删除</a></p>";
            
            obj.append(_p);

            addMarker(point , stringAddress );

            $(".js-hot-modelDelete").click(function () {
                deleteModel(this);
            });

            $(".js-hot-modelrecord").click(function () {

                var lat = $(this).attr("data-point-lat");
                var lng = $(this).attr("data-point-lng");
                var point = new BMap.Point(lng, lat);
                moveToPoint(point);
            });

        });

    };


    function addMarker(point, address) {
        var marker = new BMap.Marker(point);
        var offset = new BMap.Size(0, -20);
        var label = new BMap.Label(address);
        label.setOffset(offset);
        myMap.addOverlay(marker);
        marker.setLabel(label);
    }


    function moveToPoint(point) {

        setTimeout(function () {
            myMap.panTo(point);   //两秒后移动到指定的位置
        }, 500)

    }


    function deleteModel(obj) {

        var lat = $(obj).parent().attr("data-point-lat");//Html5可以使用$(this).data('id')方式来写;
        var lng = $(obj).parent().attr("data-point-lng");
        var address = $(obj).parent().attr('data-point-add');

        //alert('lat=' + lat + ",lng=" + lng);
        var layer = require("layer");
        layer.confirm('您确定要删除该条位置信息[经度=' + lat + '纬度=' + lng + ']' + address + '吗？', {
            btn: ['确定', '取消'] //按钮
        }, function (index) {
            var allOverlay = myMap.getOverlays();
            //console.log(allOverlay);
            //alert(allOverlay.length);
            for (var i = 0; i < allOverlay.length; i++) {
                if (allOverlay[i].getPosition() == null) continue;

                //alert(allOverlay[i].getPosition());
                //alert(allOverlay[i].getLabel().content);
                console.log(allOverlay[i].getPosition());

                if (allOverlay[i].getPosition().lat.toString() == lat && allOverlay[i].getPosition().lng.toString() == lng) {        
                    myMap.removeOverlay(allOverlay[i]);
                    $(obj).parent().remove();
                    layer.close(index);
                    return false;
                }
            }

            layer.close(index);           

        });





        //var obj = $(".js-hot-modelDelete");
        //$.each(obj, function (item, dom) {
        //    $(dom).click(function () {//绑定删除事件
        //        var lat = $(this).attr("data-point-lat");//Html5可以使用$(this).data('id')方式来写;
        //        var lng = $(this).attr("data-point-lng");
        //        var address = $(this).attr('data-point-add');

        //        alert('lat='+lat + ",lng="+lng);
        //        var layer = require("layer");
        //        layer.confirm('您确定要删除该条位置信息[经度='+ lat +'纬度='+ lng+']'+ address+'吗？', {
        //            btn: ['确定', '取消'] //按钮
        //        }, function () {
                   
        //            Toast("ffffff");
        //        });
        //    })
        //})
    }



    $("#refreshtask").click(function () {
        window.location.reload();
    })


    $("#savetask").click(function () {


        var starttime = $("#starttime").val();
        var stoptime = $("#stoptime").val();
        var sayhirate = $("#sayhirate").val();
        var sayhimaxcount = $("#sayhimaxcount").val();
        var content = $("#content").val();
        var remark = $("#remark").val();

        var obj = $(".js-hot-modelrecord");
        var layer = require("layer");

        if (starttime.length == 0) {
            layer.alert("请选择开始时间");
            return;
        }
        if (stoptime.length == 0) {
            alert.alert("请选择结束时间");
            return;
        }
        if (sayhirate.length == 0) {
            layer.alert("请输入打招呼的频率");
            return;
        }
        if (sayhimaxcount.length == 0) {
            layer.alert("请输入人数");
            return;
        }

        if (isNaN(sayhirate)) {
            layer.alert("请输入正确的频率数字");
            return;
        }

        if (isNaN(sayhimaxcount)) {
            layer.alert("请输入正确的人数");
            return;
        }


        if (content.length == 0) {
            layer.alert("请输入打招呼内容");
            return;
        }


        var data = {
            taskid: 0,
            starttime: starttime,
            stoptime: stoptime,
            sayhi: content,
            sayhirate: sayhirate,
            sayhimaxcount: sayhimaxcount,
            remark:remark,
            locations: []
        }


        //var location;
        obj.each(function (i, obj) {

            var lat = $(obj).attr("data-point-lat");//Html5可以使用$(this).data('id')方式来写;
            var lng = $(obj).attr("data-point-lng");
            var address = $(obj).attr('data-point-add');

            if (typeof (lat) == "undefined") return;
            if (typeof (lng) == "undefined") return;
            if (typeof (address) == "undefined") return;

            var json = {
                longitude: lng,
                latitude: lat,
                address: address
            }

            data.locations.push(json);
        });

        if (data.locations.length < 1) {
            layer.alert("请设置位置信息");
            return;
        }

   
        //var task = JSON.stringify(data);


        var taskid = commonUtil.getQuery("taskid");
        if (typeof (taskid) == "undefined") {

            addTask(data);
            return;
        }
        if (taskid.length < 1) {
            addTask(data );
            return;
        }


        data.taskid = taskid;

        updateTask(data);

    })

        function addTask( data ) {

        var task = JSON.stringify(data);


        $.ajax({
            url: "/AjaxHandler.aspx?action=addtask",
            data: {
                task: task
            },
            type: "POST",
            dataType: 'json',
            success: function (data) {

                //console.log(data);
               

                if (data != null) {
                    var code = parseInt(data.code);
                    switch (code) {
                        case 200:
                            layer.msg("设置成功", { time: 2000 });
                            window.location.href = "/view/tasklist.html";
                            break;
                        case 500:
                            layer.msg( data.message , { time: 2000 });
                            break;
                        default:
                            layer.msg("系统繁忙,请稍后再试...", { time: 2000 });
                            break;
                    }
                }
            },
            error: function (data) {
                console.log(data);
            }

        });

    }


    function initTaskInfo() {

        var taskid = commonUtil.getQuery("taskid");
        var layer = require("layer");


        if (typeof (taskid) == "undefined") {
            var starttime = $("#starttime");
            var stoptime = $("#stoptime");
            var temp = commonUtil.dateFormat("yyyy-MM-dd hh:mm:ss", new Date())
            starttime.val(temp);

            temp = commonUtil.dateFormat("yyyy-MM-dd 23:59:59", new Date());
            stoptime.val(temp)


            return;
        }
        if (taskid.length < 1) {
            var starttime = $("#starttime");
            var stoptime = $("#stoptime");

            var temp = commonUtil.dateFormat("yyyy-MM-dd hh:mm:ss", new Date())
            starttime.val(temp);
             
            temp = commonUtil.dateFormat("yyyy-MM-dd 23:59:59", new Date());
            stoptime.val( temp )

            return;
        }

        var starttime = $("#starttime");
        var stoptime = $("#stoptime");
        var sayhirate = $("#sayhirate");
        var content = $("#content");
        var sayhimaxcount = $("#sayhimaxcount");
        var remark = $("#remark");
        var status = $("#taskstatus");

        var deviceno = $("#deviceno");
        var osversion = $("#osversion");
        var deviceremark = $("#deviceremark");
        var brand = $("#brand");
        var devicename=$("#devicename");

        var locationDiv = $("#js-ModelList");

        var divDevice = $("#divDevice");

        $.ajax({
            url: "/AjaxHandler.aspx?action=edittask",
            data: {
                taskid: taskid
            },
            type: "POST",
            dataType: 'json',
            success: function (data ) {
                console.log(data);

                if (data.code != 200) {

                    layer.msg(data.message, { time: 2000 });
                    

                    return;
                   
                } else {
                    starttime.val( data.data.data.starttime);
                    stoptime.val( data.data.data.stoptime);
                    content.val(data.data.data.sayhi);
                    sayhirate.val(data.data.data.sayhirate);
                    sayhimaxcount.val(data.data.data.sayhimaxcount);
                    remark.val(data.data.data.remark);

                    if (data.data.data.status == 0) {
                        status.html("未开始");
                    } else if (data.data.data.status == 1) {
                        status.html("运行中");
                    } else if (data.data.data.status == 8) {
                       status.html("已完成");
                    } else {
                        status.html("未知状态");
                    }

                    //console.log(data.data.data.locations);

                    if (typeof (data.data.data.device) == "undefined" || data.data.data.device == null ) {
                        divDevice.hide();
                    } else {                    
                      
                                              
                        devicename.html(data.data.data.device.devicename);
                        deviceno.html(data.data.data.device.deviceno);
                        osversion.html(data.data.data.device.osversion);
                        brand.html(data.data.data.device.brand);
                        deviceremark.html(data.data.data.device.remark);

                        divDevice.show();
                    }

                    if (data.data.data.locations.length != 0) {
                                           

                        //var point = new BMap.Point(data.data.data.locations[0].longitude, data.data.data.locations[0].latitude);
                        //setTimeout(function () {
                        //    myMap.panTo(point);   //两秒后移动到指定的位置
                        //}, 1500)


                        for (var i = 0; i < data.data.data.locations.length; i++) {
                            var loc = data.data.data.locations[i];
                            var _p = "<p style='height:20px;cursor:pointer;' class='js-hot-modelrecord' data-point-lat=" + loc.latitude + " data-point-lng=" + loc.longitude + " data-point-add=" + loc.address + ">" + loc.address;
                            
                            if (loc.status != 0) {
                                _p += "<label style='margin-left:10px' class='txt-green-b'>已完成</label> </p>";
                            } else {
                                _p +="<a class='js-hot-modelDelete' >删除</a> </p>";
                            }


                            locationDiv.append(_p );

                            var point = new BMap.Point(loc.longitude, loc.latitude);

                            addMarker(point, loc.address);

                            $(".js-hot-modelDelete").click(function () {
                                deleteModel(this);
                            });

                            $(".js-hot-modelrecord").click(function () {

                                var lat = $(this).attr("data-point-lat");
                                var lng = $(this).attr("data-point-lng");
                                var point = new BMap.Point(lng,lat);
                                moveToPoint(point);
                            });

                        }

                        setCurrentPoint( data.data.data.status, data.data.data.locations);

                    }


                }


            }
        });
    }


    function updateTask(data) {

        var task = JSON.stringify(data);


        $.ajax({
            url: "/AjaxHandler.aspx?action=updatetask",
            data: {
                task: task
            },
            type: "POST",
            dataType: 'json',
            success: function (data) {

                console.log(data);


                if (data != null) {
                    var code = parseInt(data.code);
                    switch (code) {
                        case 200:
                            layer.msg("修改成功", { time: 2000 });
                            window.location.href = "/view/tasklist.html";
                            break;
                        case 500:
                            layer.msg(data.message, { time: 2000 });
                            break;
                        default:
                            layer.msg("系统繁忙,请稍后再试...", { time: 2000 });
                            break;
                    }
                }
            },
            error: function (data) {
                console.log(data);
            }

        });

    }


    function drawLine( locations ) {
        if (locations.length == 0) return;

        var polyline = new BMap.Polyline([], { strokeColor: "red", strokeWeight: 2, strokeOpacity: 0.5 });
        var points = [];
        for (var i = 0; i < locations.length; i++) {
            var loc = locations[i];
            var status = loc.status;
            if (status == 0) continue;
            var point = new BMap.Point(loc.longitude, loc.latitude);
            points.push(point);
        }

        polyline.setPath(points);
        myMap.addOverlay(polyline);
    }

    function setCurrentPoint( taskStatus , locations) {
        if (locations.length == 0) return;

        var loc;

        for (var i = 0; i < locations.length; i++) {
            loc = locations[i];
            var status = loc.status;
            if (status == 0) break;
        }
        if (loc == null) return;

        var allOverlay = myMap.getOverlays();
        var currPoint;

        for (var i = 0; i < allOverlay.length; i++) {
            if (allOverlay[i].getPosition() == null) continue;

            var p = allOverlay[i].getPosition();


            if (p.lat.toString() == loc.latitude && p.lng.toString() == loc.longitude) {

                currPoint = p;
                        
                if (taskStatus == 1) { 
                    var marker = allOverlay[i];
                    marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
                }

                setTimeout(function () {
                    myMap.panTo(currPoint );   //两秒后移动到指定的位置
                }, 1000)
                break;
            }

        }


    }

});
