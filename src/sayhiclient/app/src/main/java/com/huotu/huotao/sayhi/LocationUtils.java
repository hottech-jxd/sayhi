package com.huotu.huotao.sayhi;

import android.os.Environment;

import com.huotu.huotao.sayhi.bean.SayHiBean;

import de.robv.android.xposed.XSharedPreferences;
import de.robv.android.xposed.XposedBridge;

/**
 * Created by Administrator on 2017/2/21.
 */

public class LocationUtils {

    public static SayHiBean mockLocation2(){


        String path = Environment.getExternalStorageDirectory() +"/sayhi/preferences.xml";

        XposedBridge.log( "prefPath=" + path );

        StringBuilder jsonstr=new StringBuilder();
        try {
            jsonstr = FileUtils.readFile(path, "utf-8");



        }catch ( Exception ex){
            XposedBridge.log( "LocationUtils.mockLocation error");
            int count = ex.getStackTrace().length;
            for(int i=0;i<count;i++){
                XposedBridge.log( ex.getStackTrace()[i].toString() );
            }
        }

        try{
            String json = jsonstr.toString();

            XposedBridge.log("read content=" + json);

            SayHiBean bean = (SayHiBean) WechatUtils.getGson().fromJson(json, SayHiBean.class);
            return bean;
        }catch (Exception ex){
            XposedBridge.log( "LocationUtils.mockLocation222222s error");
            int count = ex.getStackTrace().length;
            for(int i=0;i<count;i++){
                XposedBridge.log( ex.getStackTrace()[i].toString() );
            }
        }
        return null;

    }


    public static SayHiBean mockLocation(){
        XSharedPreferences xSharedPreferences  =new XSharedPreferences( BuildConfig.APPLICATION_ID , BuildConfig.APPLICATION_ID+"_preferences");
        xSharedPreferences.reload();

        String lonStr = xSharedPreferences.getString(Constants.PARAMETER_LONGITUDE,"0");
        String latStr =xSharedPreferences.getString( Constants.PARAMETER_LATITUDE ,"0");
        String content = xSharedPreferences.getString(Constants.PARAMETER_CONTENT ,"");
        SayHiBean bean=new SayHiBean();
        bean.setLongitude( lonStr  );
        bean.setLatitude(  latStr );
        bean.setContent(content);

        //XposedBridge.log(" bbbb = "+ lonStr + ","+latStr );

        return bean;

    }
}
