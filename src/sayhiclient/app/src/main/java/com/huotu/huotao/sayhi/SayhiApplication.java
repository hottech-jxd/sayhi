package com.huotu.huotao.sayhi;

import android.app.Application;
import android.content.pm.PackageManager;
import android.os.Build;
import android.telephony.TelephonyManager;
import android.util.Log;

import com.huotu.huotao.sayhi.bean.DeviceBean;
import com.squareup.leakcanary.LeakCanary;

/**
 * Created by Administrator on 2017/2/23.
 */
public class SayhiApplication extends Application {
    public static String TAG = SayhiApplication.class.getName();
    static SayhiApplication instance;

    @Override
    public void onCreate() {
        super.onCreate();

        instance = this;

        if (LeakCanary.isInAnalyzerProcess(this)) {
            // This process is dedicated to LeakCanary for heap analysis.
            // You should not init your app in this process.
            return;
        }
        LeakCanary.install(this);

    }


    /**
     * 获取当前应用程序的版本号
     */
    public static String getAppVersion() {
        String version = "0";
        try {
            version = instance.getPackageManager().getPackageInfo(instance.getPackageName(), 0).versionName;
        } catch (PackageManager.NameNotFoundException e) {
            Log.e(TAG, e.getMessage() == null ? " SayHiApplication.getAppVersion Error" : e.getMessage());
        }
        return version;
    }

    public static DeviceBean getDeviceInfo() {
        DeviceBean bean = new DeviceBean();

        bean.setDevicetype(Build.MODEL);
        bean.setOsversion(Build.VERSION.RELEASE);
        bean.setDevicename(Build.DEVICE);
        bean.setBrand(Build.BRAND);

        TelephonyManager telephonyManager = (TelephonyManager) instance.getSystemService(TELEPHONY_SERVICE);


        bean.setDeviceno(telephonyManager.getDeviceId());

        bean.setDevicename(bean.getBrand() + bean.getDevicetype());

        return bean;
    }

}
