package com.huotu.huotao.sayhi;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;

import com.huotu.huotao.sayhi.bean.SayHiBean;

public class SayHiBroadcastReceiver extends BroadcastReceiver {
    public SayHiBroadcastReceiver() {
    }

    @Override
    public void onReceive(Context context, Intent intent) {
        if(context==null)return;
        if(intent==null)return;
        if(!intent.hasExtra("data"))return;
//        SayHiBean bean = (SayHiBean) intent.getSerializableExtra("data");
//        double lon = bundle.getDouble("longitude");
//        double lat = bundle.getDouble("latitude");
//        String content = bundle.getString("content");
//        PackageManager packageManager = context.getPackageManager();
//        // 获取启动微信的intent
//        Intent wechatIntent = packageManager.getLaunchIntentForPackage( Constants.WECHAT_APP_PACKAGENAME );
//        //每次启动微信应用时，但是以重新启动应用的形式打开
//        wechatIntent.setFlags( Intent.FLAG_ACTIVITY_NEW_TASK );
//        //跳转
//        context.startActivity(wechatIntent);
    }
}
