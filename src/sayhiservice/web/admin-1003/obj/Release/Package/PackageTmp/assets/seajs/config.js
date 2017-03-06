seajs.config({
	alias: {
		"jquery": "js/jquery-1.9.1.min.js",
		"bootstrap": "js/bootstrap.min.js",
		"validate": "libs/validate/jquery.validate.min.js",
		"message": "libs/validate/jquery.validate.addMethod.js",
		"common":"js/page/common.js?t=552225",
		"main": "js/page/main.js",
        "login":"js/page/login.js",
        "JGrid": "libs/JGrid/jquery.JGrid.js",
        //"area": "libs/jquery.area.js",
        "datetime": "libs/My97DatePicker/WdatePicker.js",
		"layer":"libs/layer/layer.js",
		//"signUpList": "js/page/system/signUpList.js?v=1.0.1",
        "deviceList":"js/page/sayhi/deviceList.js?v=1.0.1",
	    
	},
	preload: ['jquery']
});