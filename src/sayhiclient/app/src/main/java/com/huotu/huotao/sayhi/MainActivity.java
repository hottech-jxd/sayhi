package com.huotu.huotao.sayhi;

import android.accessibilityservice.AccessibilityServiceInfo;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.os.IBinder;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.accessibility.AccessibilityManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.huotu.huotao.sayhi.bean.BaseBean;
import com.huotu.huotao.sayhi.bean.DeviceBean;
import com.huotu.huotao.sayhi.bean.SayHiBean;
import com.huotu.huotao.sayhi.bean.TaskResultBeam;

import java.lang.reflect.Method;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.huotu.huotao.sayhi.R.id.btnSet;


public class MainActivity extends AppCompatActivity implements View.OnClickListener{
    private SettingsHelper mSettings;
    EditText etlat;
    EditText etlng;
    EditText etContent;
    EditText etMaxCount;
    EditText etRate;
    Button btnStartWechat;
    Button btnTest;
    Button btnRegister;
    Button btnTestGetTask;

    IGetDataAidlInterface iGetDataAidlInterface;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        etlat = (EditText)findViewById(R.id.edLat);
        etlng = (EditText)findViewById(R.id.edLng);
        etContent = (EditText)findViewById(R.id.etcontent);
        etMaxCount = (EditText)findViewById(R.id.etMaxCount);
        etContent.setText("hello,你好");
        etRate = (EditText)findViewById(R.id.etRate);

        mSettings = new SettingsHelper(this); // new SettingsHelper(this);
        //mSettings.setString("longitude", "116.449535");//
        //mSettings.setString("latitude", "39.862559");//

        SharedPreferences sharedPreferences = getSharedPreferences(this.getPackageName(), MODE_WORLD_READABLE);

        String json = getSharedPreferences(this.getPackageName(), MODE_WORLD_READABLE).getString(Constants.PARAMETER_SAYHIDATA,"");
        SayHiBean bean = WechatUtils.getGson().fromJson(json, SayHiBean.class);
        if( bean!=null ) {
            etContent.setText(bean.getContent());
            etlat.setText(bean.getLatitude());
            etlng.setText(bean.getLongitude());
            etMaxCount.setText(String.valueOf(bean.getMaxCount()));
            etRate.setText( String.valueOf(bean.getRate()));
        }

        //String lat = sharedPreferences.getString(  );  //mSettings.getString(Constants.PARAMETER_LATITUDE,"39.862559");
        //String lon = mSettings.getString(Constants.PARAMETER_LONGITUDE ,"116.449535");

        Button btnSet = (Button)findViewById(R.id.btnSet);
        btnSet.setOnClickListener(this);
        btnStartWechat = (Button)findViewById(R.id.btnStartwechat);
        btnStartWechat.setOnClickListener(this);

        Button btnTest=(Button) findViewById(R.id.btnTest);
        btnTest.setOnClickListener(this);

        btnRegister = ( Button)findViewById(R.id.btnregisterdevice);
        btnRegister.setOnClickListener(this);

        btnTestGetTask = (Button) findViewById(R.id.btntestgettask);
        btnTestGetTask.setOnClickListener(this);




        //initData();

    }

    @Override
    public void onClick(View view) {
        if(view.getId()== btnSet) {
            String lat = etlat.getText().toString();
            String lng = etlng.getText().toString();
            String content = etContent.getText().toString();
            String strMaxCount = etMaxCount.getText().toString();
            String strRate = etRate.getText().toString();

            if(lat.isEmpty()){
                Toast.makeText(this,"请输入纬度",Toast.LENGTH_LONG).show();
                return;
            }
            if(lng.isEmpty()){
                Toast.makeText(this,"请输入经度",Toast.LENGTH_LONG).show();
                return;
            }
            if(content.isEmpty()){
                Toast.makeText(this,"请输入打招呼内容",Toast.LENGTH_LONG).show();
                return;
            }
            if( strMaxCount.isEmpty()){
                Toast.makeText(this,"请输入最大人数",Toast.LENGTH_LONG).show();
                return;
            }
            int maxCount=0;
            try {
                maxCount = Integer.valueOf(strMaxCount);
            }catch (NumberFormatException ex){
                Toast.makeText(this,"请输入正确的数字",Toast.LENGTH_LONG).show();
                return;
            }

            if( maxCount<1){
                Toast.makeText(this,"输入的值太小了",Toast.LENGTH_LONG).show();
                return;
            }

            if(strRate.isEmpty()){
                Toast.makeText(this,"请输入频率",Toast.LENGTH_LONG).show();
                return;
            }

            int rate =0;
            try {
                rate = Integer.valueOf(strRate);
            }catch (NumberFormatException ex){
                Toast.makeText(this,"请输入正确的数字",Toast.LENGTH_LONG).show();
                return;
            }
            if( rate<5){
                Toast.makeText(this,"请输入大于5秒的数字",Toast.LENGTH_LONG).show();
                return;
            }

            //mSettings.setString(Constants.PARAMETER_LONGITUDE , lon);//
            //mSettings.setString(Constants.PARAMETER_LATITUDE , lat);//

            SayHiBean bean = new SayHiBean();
            bean.setLatitude(lat);
            bean.setLongitude(lng);
            bean.setContent( content);
            bean.setMaxCount( maxCount );
            bean.setRate(rate);

            String json =  WechatUtils.getGson().toJson(  bean );

            SharedPreferences.Editor editor= this.getSharedPreferences(this.getPackageName() , MODE_WORLD_READABLE ).edit();   //getSharedPreferences(getPackageName(), MODE_WORLD_READABLE).edit();
            editor.putString(Constants.PARAMETER_SAYHIDATA , json);
            editor.commit();

            Toast.makeText(this, "设置完成", Toast.LENGTH_LONG).show();
        }else if(view.getId()==R.id.btnStartwechat){
            PackageManager packageManager = this.getPackageManager();
            // 获取启动微信的intent
            Intent intent = packageManager.getLaunchIntentForPackage( Constants.WECHAT_APP_PACKAGENAME );
            //每次启动微信应用时，但是以重新启动应用的形式打开
            intent.setFlags( Intent.FLAG_ACTIVITY_NEW_TASK );
            //跳转
            startActivity(intent);

        }else if(view.getId()==R.id.btnTest){


            checkAccessibilityServiceIsEnabled();
             //testService();

            //SettingsHelper t = new SettingsHelper(BuildConfig.APPLICATION_ID);

            //t.reload();

            //String msg =t.getString(Constants.PARAMETER_SAYHIDATA,"");
            //Toast.makeText(this, msg ,Toast.LENGTH_LONG).show();


            String msg = this.getSharedPreferences(this.getPackageName()+"_preferences" , MODE_WORLD_READABLE).getString(Constants.PARAMETER_SAYHIDATA,"");

            etContent.setText( msg );

           //SharedPreferences sharedPreferences = this.getSharedPreferences( getPackageName() , MODE_WORLD_READABLE );


            //PreferenceManager.getDefaultSharedPreferences(this);

        }else if(view.getId() == R.id.btnregisterdevice){
            register();
        }else if(view.getId() == R.id.btntestgettask){
            testGetTask();
        }
    }

    private void testGetTask(){
        DeviceBean device = SayhiApplication.getDeviceInfo();
        String deviceno = device.getDeviceno();
        String starttime = Utils.formatDate(System.currentTimeMillis(),"yyyy-MM-dd 00:00:00");
        Date end =new Date();
        // Calendar.getInstance(Locale.getDefault()).get(Calendar.YEAR);
        String stoptime =  Utils.formatDate( end.getTime() , "yyyy-MM-dd 23:59:59" );

        Map<String,String> param = new HashMap<>();
        param.put("deviceno", deviceno);
        param.put("starttime",  starttime );
        param.put("stoptime", stoptime );

        Call<TaskResultBeam> call = ZRetrofitUtil.getApiDefine().getTaskInfo( param);
        call.enqueue(new Callback<TaskResultBeam>() {
            @Override
            public void onResponse(Call<TaskResultBeam> call, Response<TaskResultBeam> response) {
                if( response.code()!=200){
                    Toast.makeText(MainActivity.this, response.message(),Toast.LENGTH_LONG).show();
                    return;
                }
                if(response.body()==null){
                    Toast.makeText(MainActivity.this,"请求出错",Toast.LENGTH_LONG).show();
                    return;
                }

                Toast.makeText(MainActivity.this, response.body().getMessage(),Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<TaskResultBeam> call, Throwable t) {
                Toast.makeText(MainActivity.this,"请求出错",Toast.LENGTH_LONG).show();
            }
        });
    }

    private void register(){
        DeviceBean bean = SayhiApplication.getDeviceInfo();

        Map<String,String> param = new HashMap<>();
        param.put("deviceno", bean.getDeviceno());
        param.put("devicename",bean.getDevicename());
        param.put("devicetype",bean.getDevicetype());
        param.put("osversion",bean.getOsversion());
        param.put("brand",bean.getBrand());

        Call<BaseBean> call = ZRetrofitUtil.getApiDefine().addDevice( param );
        call.enqueue(new Callback<BaseBean>() {
            @Override
            public void onResponse(Call<BaseBean> call, Response<BaseBean> response) {
                if( response.code()!=200){
                    Toast.makeText(MainActivity.this, response.message(),Toast.LENGTH_LONG).show();
                    return;
                }
                if(response.body()==null){
                    Toast.makeText(MainActivity.this,"请求出错",Toast.LENGTH_LONG).show();
                    return;
                }

                Toast.makeText(MainActivity.this, response.body().getMessage(),Toast.LENGTH_LONG).show();
            }

            @Override
            public void onFailure(Call<BaseBean> call, Throwable t) {
                Toast.makeText(MainActivity.this,"请求出错",Toast.LENGTH_LONG).show();
            }
        });

    }

    private void testService(){
        try {
            if (iGetDataAidlInterface == null) {
                Class<?> ServiceManager = Class.forName("android.os.ServiceManager");
                Method getService = ServiceManager.getDeclaredMethod("getService", String.class);
                IBinder iBinder =  (IBinder) getService.invoke(null, SayHiGetDataService.getServiceName());
                iGetDataAidlInterface = IGetDataAidlInterface.Stub.asInterface( iBinder );
            }

           String result =  iGetDataAidlInterface.getData();

            Toast.makeText(this , result , Toast.LENGTH_LONG).show();

        }catch (Exception ex){
            Log.e("sss" , ex.getMessage());
        }



    }


//    private void initData(){
//        Intent intent = this.getIntent();
//        if(intent==null)return;
//        if(!intent.hasExtra("data"))return;
//        SayHiBean bean =(SayHiBean) intent.getSerializableExtra("data");
//        if(bean==null)return;
//
//        SharedPreferences.Editor editor= getSharedPreferences(this.getPackageName(), MODE_WORLD_READABLE).edit();// getDefaultSharedPreferences(this).edit();
//
//
//        editor.putString(Constants.PARAMETER_LONGITUDE , String.valueOf( bean.getLongitude()));
//        editor.putString( Constants.PARAMETER_LATITUDE , String.valueOf( bean.getLatitude()));
//        editor.putString(Constants.PARAMETER_CONTENT , String.valueOf(bean.getContent()));
//        editor.putInt(Constants.PARAMETER_MAXCOUNT, bean.getMaxCount());
//        String json = new Gson().toJson( bean );
//        editor.putString(Constants.PARAMETER_SAYHIDATA, json );
//
//        editor.commit();
//
//
//        PackageManager packageManager = getPackageManager();
//        // 获取启动微信的intent
//        Intent wechatIntent = packageManager.getLaunchIntentForPackage( Constants.WECHAT_APP_PACKAGENAME );
//        //每次启动微信应用时，但是以重新启动应用的形式打开
//        wechatIntent.setFlags( Intent.FLAG_ACTIVITY_NEW_TASK );
//        //跳转
//        startActivity(wechatIntent);
//
//        this.finish();
//
//    }
//


    private boolean checkAccessibilityServiceIsEnabled(){
        AccessibilityManager accessibilityManager = (AccessibilityManager) getSystemService(Context.ACCESSIBILITY_SERVICE);
        List<AccessibilityServiceInfo> accessibilityServices =
                accessibilityManager.getEnabledAccessibilityServiceList(AccessibilityServiceInfo.FEEDBACK_GENERIC);

        String serviceName = SayHiToNearPersonService.class.getSimpleName();

        for (AccessibilityServiceInfo info : accessibilityServices) {
            if (info.getId().equals(getPackageName() + "/."+ serviceName)) {
                return true;
            }
        }

        LogUtils.log("-------微信打招呼插件没有启动------");
        return false;
    }

}
