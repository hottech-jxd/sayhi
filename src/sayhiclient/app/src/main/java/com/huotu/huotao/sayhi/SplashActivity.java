package com.huotu.huotao.sayhi;

import android.accessibilityservice.AccessibilityServiceInfo;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.os.PowerManager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.accessibility.AccessibilityManager;

import com.huotu.huotao.sayhi.bean.BaseBean;
import com.huotu.huotao.sayhi.bean.SayHiBean;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class SplashActivity extends AppCompatActivity {



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash);

        initData();

        checkAccessibilityServiceIsEnabled();

    }

    private void initData(){
        if( getIntent()==null ||  !getIntent().hasExtra(Constants.PARAMETER_TASK_DATA)) {
            Intent intent = new Intent();
            intent.setClass(this , MainActivity.class);
            this.startActivity(intent);
            this.finish();
            return;
        }


        //获得新的任务配置信息
        SayHiBean newConfig = (SayHiBean) getIntent().getSerializableExtra(Constants.PARAMETER_TASK_DATA);
        //获得当前的任务配置信息
        SayHiBean currentConfig=null;
        try {
            String currentConfigJson = getSharedPreferences(getPackageName(), MODE_WORLD_READABLE).getString(Constants.PARAMETER_SAYHIDATA, "");
            currentConfig = WechatUtils.getGson().fromJson(currentConfigJson, SayHiBean.class);
        }catch (Exception ex){
        }
        //比较两个任务是否相同
        boolean isExist = SayHiCache.judgeExistConfig( newConfig ,  currentConfig );
        if(isExist){//相同，判断当前配置任务是否已经完成，没有完成则，启动微信 不修改配置
            LogUtils.log("当前任务已经存在,");

            boolean isFinished = SayHiCache.judgeCurrentConfigIsFinished( currentConfig );
            if(!isFinished){//还没有完成则，启动微信
                if(  needUpdateCurrentConfig( currentConfig , newConfig )){//如果任务相同，则判断 最大人数，频率，打招呼内容是否有更新，有则修改以后保存配置
                    LogUtils.log("当前任务配置有更新");
                    saveNewConfig(currentConfig);
                }else {
                    LogUtils.log("当前任务只启动微信");
                    startWechatApp();
                }
                return;
            }else{//如果任务完成，则判断状态是否上报成功。
                if(!currentConfig.isReportStatusSuccess()){//
                    LogUtils.log("-----start update status");
                    SayHiCache.updateTaskLocationStatus( SplashActivity.this , currentConfig , "" );
                    return;
                }
                return;
            }
        }

        //不相同，则先调用接口更新任务的状态为运行状态，如果接口返回成功，则保存新的配置信息，启动微信，进行打招呼操作
        //如果接口调用失败，则关闭app，不进行打招呼操作。
        updateTaskStatusRun(newConfig);
    }

    /***
     *
     * @param currentConfig
     * @param newConfig
     */
    private boolean needUpdateCurrentConfig(SayHiBean currentConfig , SayHiBean newConfig ){
        if( currentConfig==null || newConfig==null ) return false;
        boolean isUpdate=false;
        if( currentConfig.getContent() !=null && newConfig.getContent()!=null && !currentConfig.getContent().equals( newConfig.getContent() )){
            currentConfig.setContent( newConfig.getContent() );
            isUpdate = true;
        }
        if( currentConfig.getMaxCount()!= newConfig.getMaxCount()){
            currentConfig.setMaxCount( newConfig.getMaxCount());
            isUpdate = true;
        }
        if(currentConfig.getRate() != newConfig.getRate()){
            currentConfig.setRate( newConfig.getRate() );
            isUpdate = true;
        }
        return isUpdate;
    }

    private void saveNewConfig( SayHiBean newConfig ){
        SharedPreferences.Editor editor =  this.getSharedPreferences( getPackageName() , MODE_WORLD_READABLE ).edit();
        String json = WechatUtils.getGson().toJson( newConfig);
        editor.putString( Constants.PARAMETER_SAYHIDATA ,  json );
        editor.commit();

        startWechatApp();
    }


    private boolean startWechatApp(){
        PackageManager packageManager = getPackageManager();
        // 获取启动微信的intent
        Intent wechatIntent = packageManager.getLaunchIntentForPackage( Constants.WECHAT_APP_PACKAGENAME );
        //每次启动微信应用时，但是以重新启动应用的形式打开
        wechatIntent.setFlags( Intent.FLAG_ACTIVITY_NEW_TASK );
        //跳转
        startActivity(wechatIntent);
        finish();
        return true;
    }

    /***
     * 调用接口更新任务的状态为运行状态
     * @param sayHiBean
     */
    private void updateTaskStatusRun(final SayHiBean sayHiBean){
        if(sayHiBean==null) {
            this.finish();
            return;
        }

        int taskid = sayHiBean.getTaskid();
        Map<String,String> params = new HashMap<>();
        params.put("taskid", String.valueOf(taskid));
        Call<BaseBean> call = ZRetrofitUtil.getApiDefine().UpdateTaskStatusRun( params );
        call.enqueue(new Callback<BaseBean>() {
            @Override
            public void onResponse(Call<BaseBean> call, Response<BaseBean> response) {
                if( SplashActivity.this ==null ) return;

                if( response.body()==null){
                    SplashActivity.this.finish();
                    return;
                }

                if( response.body().getCode() !=200){
                    LogUtils.log( response.message());
                    SplashActivity.this.finish();
                    return;
                }

                LogUtils.log("完成更新任务("+sayHiBean.getTaskid()+")的状态为运行状态\r\n");

                saveNewConfig( sayHiBean );
            }

            @Override
            public void onFailure(Call<BaseBean> call, Throwable t) {
                if( SplashActivity.this ==null )return;
                LogUtils.log( "调用updatetaskstatusrun接口发送错误" + t.getMessage()==null?"":t.getMessage() ,  t );
                SplashActivity.this.finish();
            }
        });

    }

    /***
     * 检测微信打招呼插件是否启用
     */
    private boolean checkAccessibilityServiceIsEnabled(){
        AccessibilityManager accessibilityManager = (AccessibilityManager) getSystemService(Context.ACCESSIBILITY_SERVICE);
        List<AccessibilityServiceInfo> accessibilityServices =
                accessibilityManager.getEnabledAccessibilityServiceList(AccessibilityServiceInfo.FEEDBACK_GENERIC);

        String serviceName = SayHiToNearPersonService.class.getSimpleName();

        for (AccessibilityServiceInfo info : accessibilityServices) {
            if (info.getId().equals(getPackageName() + "/."+serviceName)) {
                return true;
            }
        }

        LogUtils.log("-------微信打招呼插件没有启动------");
        return false;
    }

}
