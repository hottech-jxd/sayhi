package com.huotu.huotao.sayhi;

import android.accessibilityservice.AccessibilityService;
import android.app.KeyguardManager;
import android.content.SharedPreferences;
import android.os.PowerManager;
import android.util.Log;
import android.view.accessibility.AccessibilityEvent;
import android.view.accessibility.AccessibilityNodeInfo;

import com.huotu.huotao.sayhi.bean.SayHiBean;

import static android.R.attr.key;
import static java.lang.System.currentTimeMillis;

/**
 * 微信辅助操作类
 * 监听微信app，自动点击“发现”tab页中的“附近的人”按钮，
 * 进入附件的人列表界面，对陌生的人，打招呼。
 * Created by Administrator on 2017/2/16.
 */
public class SayHiToNearPersonService extends AccessibilityService implements SharedPreferences.OnSharedPreferenceChangeListener{
    static WechatUtils wechatUtils;

    SharedPreferences sharedPreferences;

    SayHiBean sayHiBean;

    static PowerManager.WakeLock wakeLock;

    static KeyguardManager.KeyguardLock keyguardLock;

    @Override
    protected void onServiceConnected() {
        super.onServiceConnected();

        try {

            acquireWakeLock();


            if( wechatUtils==null ) {
                wechatUtils = new WechatUtils(this);
            }

            sharedPreferences = this.getSharedPreferences(this.getPackageName(), MODE_WORLD_READABLE);
            sharedPreferences.registerOnSharedPreferenceChangeListener(this);
            try {
                String json = sharedPreferences.getString(Constants.PARAMETER_SAYHIDATA, "");
                sayHiBean = WechatUtils.getGson().fromJson(json, SayHiBean.class);
                //if (sayHiBean == null) return;
            } catch (Exception ex) {
                sayHiBean = null;
            }

            wechatUtils.setCurrentSayhiData(sayHiBean);

            String currentDate = Utils.formatDate(System.currentTimeMillis());
            LogUtils.log("-----------" + currentDate + "微信打招呼插件连接了------------");
        }catch (Exception ex){
            String currentDate = Utils.formatDate(System.currentTimeMillis());
            LogUtils.log("-----------" + currentDate + "微信打招呼插件连接错误了------------");
            LogUtils.log(ex);
        }
    }

    @Override
    public void onAccessibilityEvent(AccessibilityEvent accessibilityEvent) {
        try {
            AccessibilityNodeInfo rootNode = this.getRootInActiveWindow();
            if ( rootNode == null) return;
            if(sayHiBean==null){
                LogUtils.log("提供的打招呼的信息为空,无法进行打招呼操作");
                this.performGlobalAction(AccessibilityService.GLOBAL_ACTION_BACK);
                return;
            }
            if ( sayHiBean.getMaxCount() < 1) {
                LogUtils.log( "可以打招呼的最大人数是0，因此无需打招呼");
                this.performGlobalAction(AccessibilityService.GLOBAL_ACTION_BACK);
                return;
            }
            if ( sayHiBean.getContent() == null || sayHiBean.getContent().isEmpty()) {
                LogUtils.log("打招呼的内容是空，无需打招呼");
                this.performGlobalAction(AccessibilityService.GLOBAL_ACTION_BACK);
                return;
            }
            if( sayHiBean.getStatus() == Constants.TASK_LOCATION_STATUS_FINISHED ){
                String json = WechatUtils.getGson().toJson(sayHiBean);
                LogUtils.log("当前位置的打招呼操作已经完成!位置信息"+ json );
                this.performGlobalAction(AccessibilityService.GLOBAL_ACTION_BACK);
                return;
            }

            Log.i(SayHiToNearPersonService.class.getName(),
                    "EventType=" + String.valueOf(accessibilityEvent.getEventType()) +
                            " PackageName=" + accessibilityEvent.getPackageName() +
                            " ClassName=" + accessibilityEvent.getClassName());
            Log.i(SayHiToNearPersonService.class.getName(),
                    "AccessibilityEvent.Source =" + (accessibilityEvent.getSource() == null ? "empty" : accessibilityEvent.getSource()) +
                            " RootInActiveWindow=" + rootNode.toString());

            int eventType = accessibilityEvent.getEventType();

//            wechatUtils.sayHi(this.getRootInActiveWindow(), accessibilityEvent);

            switch (eventType) {
                case AccessibilityEvent.TYPE_WINDOW_STATE_CHANGED:
                case AccessibilityEvent.TYPE_WINDOW_CONTENT_CHANGED:
                case AccessibilityEvent.TYPE_VIEW_FOCUSED:
                case AccessibilityEvent.TYPE_VIEW_SCROLLED:
                    wechatUtils.sayHi( rootNode , accessibilityEvent);
                    break;
                default:
                    break;
            }
        } catch (Exception ex) {
            //ex.printStackTrace();
            LogUtils.log( ex );
        }
    }

    @Override
    public void onInterrupt() {
            long datelong = currentTimeMillis();
            String currentDate = Utils.formatDate(datelong);
            String fileName = Utils.formatDate(datelong, Utils.DATE_FORMAT) + "-module";
            LogUtils.log("---------" + currentDate + " 哇哦！微信打招呼插件断开了！---------", fileName);

    }


    @Override
    public void onSharedPreferenceChanged(SharedPreferences sharedPreferences, String key) {
        try {
            if (key.equals(Constants.PARAMETER_SAYHIDATA)) {
                try {
                    String json = sharedPreferences.getString(Constants.PARAMETER_SAYHIDATA, "");
                    LogUtils.log("-------打招呼的配置信息改变------\r\n" + json);
                    sayHiBean = WechatUtils.getGson().fromJson(json, SayHiBean.class);
                } catch (Exception ex) {
                    sayHiBean = null;
                }
                wechatUtils.setCurrentSayhiData(sayHiBean);
            }
        } catch (Exception ex) {
            LogUtils.log( ex );
            ex.printStackTrace();
        }
    }


    @Override
    public void onDestroy() {
        super.onDestroy();

        releaseWeakLock();

        String currentDate = Utils.formatDate(System.currentTimeMillis());
        LogUtils.log("-----------"+currentDate+"微信打招呼插件销毁了onDestroy()------------");
    }

    @Override
    public void onLowMemory() {
        super.onLowMemory();

        String currentDate = Utils.formatDate(System.currentTimeMillis());
        LogUtils.log("-----------"+currentDate+"微信打招呼插件内存低了onLowMemory()------------");
    }

    @Override
    public void onTrimMemory(int level) {
        super.onTrimMemory(level);

        String currentDate = Utils.formatDate(System.currentTimeMillis());
        LogUtils.log("-----------"+currentDate+"微信打招呼插件onTrimMemory------------");
    }

    /***
     * 获得锁
     */
    private void acquireWakeLock(){
        try {
            //保持屏幕的常亮
            if (wakeLock == null) {
                PowerManager powerManager = (PowerManager) getSystemService(POWER_SERVICE);
                wakeLock = powerManager.newWakeLock(PowerManager.ACQUIRE_CAUSES_WAKEUP | PowerManager.FULL_WAKE_LOCK, SayHiToNearPersonService.class.getName());
                wakeLock.acquire();
            }
            //解锁屏幕
            if (keyguardLock == null) {
                KeyguardManager keyguardManager = (KeyguardManager) getSystemService(KEYGUARD_SERVICE);
                keyguardLock = keyguardManager.newKeyguardLock("unlock");
            }
            keyguardLock.disableKeyguard();
        }catch (Exception ex){
            LogUtils.log(ex);
        }
    }

    /***
     * 释放锁
     */
    private void releaseWeakLock(){
        if( wakeLock !=null && wakeLock.isHeld()){
            wakeLock.release();
            wakeLock=null;
        }
        //if( keyguardLock !=null ){
            //keyguardLock.reenableKeyguard();
        //}
    }
}
