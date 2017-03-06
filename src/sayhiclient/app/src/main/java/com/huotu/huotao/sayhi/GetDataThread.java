package com.huotu.huotao.sayhi;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.os.Handler;
import android.telephony.TelephonyManager;

import com.huotu.huotao.sayhi.bean.SayHiBean;
import com.huotu.huotao.sayhi.bean.TaskResultBeam;

import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.Random;
import de.robv.android.xposed.XposedBridge;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static android.content.Context.MODE_WORLD_READABLE;
import static android.content.Context.TELEPHONY_SERVICE;

/**
 * Created by Administrator on 2017/2/21.
 */
public class GetDataThread  extends Thread {
    private Context context;
    private Handler mHandler;
    private static SettingsHelper mSettings = new SettingsHelper(BuildConfig.APPLICATION_ID);


    public GetDataThread(Context context) {

        XposedBridge.log("----------getdatathread contructor---------");

        this.context = context;
    }

    public void startRun() {
        this.run();
    }

    void recyle(){

        mHandler.postDelayed(new Runnable() {
            @Override
            public void run() {
                getDataByHttp();
                mHandler.postDelayed(this, Constants.Request_SAYHI_Task_PERIOD );
            }
        }, Constants.Request_SAYHI_Task_PERIOD );
    }

    private void testData(){
        String dateString = Utils.formatDate(System.currentTimeMillis());

        double lon = 116.449535;//117.8621788;
        double lat = 39.862559;//24.41584488;

        Random random = new Random();
        random.nextDouble();
        double lonlon = lon + 2 * random.nextDouble();
        double latlat = lat + 2 * random.nextDouble();

        SayHiBean bean = new SayHiBean();
        bean.setLongitude( String.valueOf( lonlon)) ;
        bean.setLatitude( String.valueOf( latlat));
        bean.setContent("hello,你好！");
        bean.setMaxCount(20);

        //intent.putExtra( Constants.PARAMETER_TASK_DATA , bean);

        //context.sendBroadcast(intent);

        startApp( bean );

        XposedBridge.log("date="+ dateString+"---longitude= " + String.valueOf(lonlon) + " latitude=" + String.valueOf(latlat));
    }


    private void startApp( SayHiBean bean){

        //String prefsPath = Environment.getExternalStorageDirectory() + "/sayhi/preferences.xml";
        //XposedBridge.log("prefpath=" + prefsPath);

        //File prefsFile = new File( prefsPath );
        //prefsFile.setReadable(true, false);

//        try {
//
//            String json = new Gson().toJson(bean);
//            XposedBridge.log( "json=" + json );
//            FileUtils.writeFile(prefsPath, json);
//        }catch ( Exception ex ){
//            XposedBridge.log("startWechat save xml error");
//            XposedBridge.log( ex.getMessage() );
//
//            int count = ex.getStackTrace().length;
//            for(int i=0;i<count;i++) {
//                XposedBridge.log(ex.getStackTrace()[i].toString());
//            }
//        }

        //String sharePath =  BuildConfig.APPLICATION_ID + "_preferences";

        // this.context.getPackageName();

        //XposedBridge.log("sharePath=" + sharePath);
        //XposedBridge.log( "getdatethread = " + context.getPackageName() );
        //SharedPreferences preferences = context.getSharedPreferences( sharePath  , Context.MODE_WORLD_READABLE);
        //SharedPreferences.Editor editor= preferences.edit();
        //editor.putString("longitude", String.valueOf( bean.getLongitude()));
        //editor.putString("latitude",String.valueOf(bean.getLatitude()));
        //editor.commit();


        PackageManager packageManager = context.getPackageManager();
        // 获取启动微信的intent
        Intent huotaoIntent = packageManager.getLaunchIntentForPackage(BuildConfig.APPLICATION_ID);
        //每次启动微信应用时，但是以重新启动应用的形式打开
        huotaoIntent.setFlags( Intent.FLAG_ACTIVITY_NEW_TASK );
        huotaoIntent.putExtra( Constants.PARAMETER_TASK_DATA , bean);
        //跳转
        context.startActivity(huotaoIntent);
    }

    @Override
    public void run() {
        super.run();

        XposedBridge.log("---------getdatathread--------");
        //Looper.prepare();
        mHandler = new Handler();
        recyle();

        XposedBridge.log("-------getdatathreadend---------");

        //Looper.loop();

//        while (true) {
//            try {
//                Thread.sleep( 10 * 1000);
//            } catch (Exception ex) {
//            }


    }


    private void startApp(TaskResultBeam resultBeam){
        if(resultBeam==null) {
            return;
        }
        if( resultBeam.getData()==null ){
            XposedBridge.log("任务空");
            return;
        }
        if(resultBeam.getData().getLocations()==null || resultBeam.getData().getLocations().size() <1 ){
            XposedBridge.log("位置信息空");
            return;
        }

        SharedPreferences.Editor editor = context.getSharedPreferences(context.getPackageName(), MODE_WORLD_READABLE).edit();
        String json = WechatUtils.getGson().toJson(  resultBeam );
        editor.putString( Constants.PARAMETER_TASK_DATA , json);
        editor.commit();

        SayHiBean sayHiBean = new SayHiBean();
        sayHiBean.setMaxCount(resultBeam.getData().getSayhimaxcount());
        sayHiBean.setRate( resultBeam.getData().getSayhirate() );
        sayHiBean.setContent( resultBeam.getData().getSayhi() );
        sayHiBean.setLongitude( resultBeam.getData().getLocations().get(0).getLongitude() );
        sayHiBean.setLatitude( resultBeam.getData().getLocations().get(0).getLatitude());
        sayHiBean.setStatus( resultBeam.getData().getLocations().get(0).getStatus() );
        sayHiBean.setLocationid( resultBeam.getData().getLocations().get(0).getLocationid() );
        sayHiBean.setTaskid( resultBeam.getData().getTaskid());

        startApp(sayHiBean);
    }


    private void getDataByHttp(){
        try {
            XposedBridge.log("-----start http request----------");

            ApiDefine apiDefine = ZRetrofitUtil.getApiDefine();

            TelephonyManager telephonyManager = (TelephonyManager) context.getSystemService(TELEPHONY_SERVICE);
            String deviceno = telephonyManager.getDeviceId();
            String starttime = Utils.formatDate(System.currentTimeMillis(), "yyyy-MM-dd 00:00:00");
            Date end =new Date();
            String stoptime =  Utils.formatDate( end.getTime() , "yyyy-MM-dd 23:59:59" );

            Map<String, String> map = new HashMap<>();
            map.put("deviceno", deviceno);
            map.put("starttime",  starttime );
            map.put("stoptime", stoptime );
            Call<TaskResultBeam> call = apiDefine.getTaskInfo( map );

            XposedBridge.log("----http request called----");

            call.enqueue(new Callback<TaskResultBeam>() {
                @Override
                public void onResponse(Call<TaskResultBeam> call, Response<TaskResultBeam> response) {
                    String msg = WechatUtils.getGson().toJson( response.body() );
                    XposedBridge.log(msg);
                    startApp(response.body());
                }

                @Override
                public void onFailure(Call<TaskResultBeam> call, Throwable t) {
                    XposedBridge.log(" 请求数据出错了！ " + t.getMessage() == null ? "" : t.getMessage());
                }
            });
        }catch (Exception ex){
            XposedBridge.log( " test() "+ ex.getMessage()==null?"":ex.getMessage() );
            int count = ex.getStackTrace().length;
            for(int i=0;i<count;i++){
                XposedBridge.log( ex.getStackTrace()[i].toString() );
            }
        }
    }
}
