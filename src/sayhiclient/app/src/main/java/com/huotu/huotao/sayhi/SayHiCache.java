package com.huotu.huotao.sayhi;

import android.accessibilityservice.AccessibilityService;
import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

import com.huotu.huotao.sayhi.bean.BaseBean;
import com.huotu.huotao.sayhi.bean.NearPersonBean;
import com.huotu.huotao.sayhi.bean.SayHiBean;
import com.huotu.huotao.sayhi.bean.SayHiResultBean;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.StringTokenizer;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by Administrator on 2017/2/22.
 */
public class SayHiCache implements Serializable {
    static List<SayHiResultBean> cache;

    public SayHiCache(){
    }

    public static void addSayHi( SayHiResultBean bean ) {
        if (cache == null) cache = new ArrayList<>();
        if (bean == null) return;
        cache.add(bean);
    }

    public static boolean hasSayHi(SayHiResultBean bean){
        if( cache==null || cache.size()<1) return false;
        if(bean==null) return false;
        for(SayHiResultBean item : cache){
            if( item.getSayHiBean().getLatitude().equals( bean.getSayHiBean().getLatitude() )
                    && item.getSayHiBean().getLongitude().equals( bean.getSayHiBean().getLongitude() ) ) return true;
        }
        return false;
    }

    public static boolean canSayHi( SayHiBean current){
        if( current ==null) return false;
        SayHiResultBean clone = new SayHiResultBean();
        clone.setSayHiBean(current);

        boolean canOperate = hasSayHi(clone);
        if( !canOperate )return false;

        //如果状态已经是完成状态则不可以在操作
        if( current.getStatus() == Constants.TASK_LOCATION_STATUS_FINISHED ) return false;

        for( SayHiResultBean item : cache ){
            SayHiBean model = item.getSayHiBean();
            if(model.getLongitude().equals( current.getLongitude()) && model.getLatitude().equals( current.getLatitude() )){
                if( item.isNoFindNearPerson() ) return false;
                if( item.getNearPersons()==null || item.getNearPersons().size() < current.getMaxCount()) return true;
            }
        }
        return false;
    }

    public static SayHiResultBean getSayHiResult(SayHiBean bean){
        if( bean==null)return null;
        if(cache==null)return null;
        for(SayHiResultBean item : cache){
            if(  item.getSayHiBean().getLatitude().equals( bean.getLatitude() )
                    && item.getSayHiBean().getLongitude().equals( bean.getLongitude() ) ) return item;
        }
        return null;
    }

    public static void addNearPerson( SayHiBean sayHiBean , NearPersonBean nearPersonData){
        SayHiResultBean data = getSayHiResult(sayHiBean);
        if( data ==null) return;

        if( data.getNearPersons()==null){
            data.setNearPersons( new ArrayList<NearPersonBean>());
        }
        data.getNearPersons().add( nearPersonData );
    }

    public static boolean isTaskFinished(SayHiBean bean , AccessibilityService accessibilityService ){
        if( bean==null) return true;
        if(bean.getStatus()== Constants.TASK_LOCATION_STATUS_FINISHED ) return true;//标记该位置已经完成

        SayHiResultBean resultBean = getSayHiResult( bean);
        if( resultBean==null) return true;
        //当该位置，无法找到附件的人时，则判断为完成状态，并且调用接口上报状态
        if( resultBean.isNoFindNearPerson()){
            bean.setStatus( Constants.TASK_LOCATION_STATUS_FINISHED );
            //saveConfig( accessibilityService , resultBean ,"无法找到附近的人");
            updateTaskLocationStatus(accessibilityService, bean , "");
            return true;
        }
        //当该位置，已经完成微信可以打招呼的所有的人时，则判断为任务完成，并且调用接口上报状态
        if(resultBean.isReachedWechatMaxCount()){
            bean.setStatus(Constants.TASK_LOCATION_STATUS_FINISHED);
            //saveConfig( accessibilityService , resultBean , "");
            updateTaskLocationStatus(accessibilityService , bean, "");
            return true;
        }
        //当该位置的打招呼人数大于设置的最大值时，则判断为完成状态，并且调用接口上报状态
        if( resultBean.getNearPersons() !=null && resultBean.getNearPersons().size()>= bean.getMaxCount() ){
            bean.setStatus( Constants.TASK_LOCATION_STATUS_FINISHED );
            //saveConfig(accessibilityService, resultBean , "" );
            updateTaskLocationStatus( accessibilityService , bean , "" );
            return true;
        }
        return false;
    }

    private static void saveConfig(Context context , SayHiBean bean ){

        SharedPreferences.Editor editor = context.getSharedPreferences(context.getPackageName(), Context.MODE_WORLD_READABLE).edit();
        String json = WechatUtils.getGson().toJson(  bean );
        editor.putString( Constants.PARAMETER_SAYHIDATA ,  json);
        editor.commit();
    }


    public static void updateTaskLocationStatus(final Context context  , final SayHiBean bean , String remark ){
        Map<String,String> param = new HashMap<>();
        param.put("taskid", String.valueOf( bean.getTaskid()));
        param.put("locationid", String.valueOf( bean.getLocationid()));
        param.put("remark", remark );

        Call<BaseBean> call = ZRetrofitUtil.getApiDefine().updateTaskLocationStatus(param);
        call.enqueue(new Callback<BaseBean>() {
            @Override
            public void onResponse(Call<BaseBean> call, Response<BaseBean> response) {
                Log.i(SayHiCache.class.getName(), response.toString() );
                String msg = "code=" + String.valueOf(response.code());
                msg +=",message="+ response.message();
                msg +=  response.body()!=null?  ("code="+ response.body().getCode()+" "+ response.body().getMessage()) : "";
                LogUtils.log( "请求updateTaskLocationStatus接口返回的结果:"+ msg );
                if(  response.code() != 200){
                    return;
                }

                bean.setReportStatusSuccess(true);
                saveConfig( context , bean);
            }

            @Override
            public void onFailure(Call<BaseBean> call, Throwable t) {
                LogUtils.log( "请求updateTaskLocationStatus接口报错了" ,t.getStackTrace());
                bean.setReportStatusSuccess(false);
            }
        });
    }

    public static void removeAll(){
        if( cache==null) return;
        cache.clear();
    }

    /***
     * 判断新的配置信息是否已经存在
     * 判断条件是 比较经纬度，如果存在，则更新 打招呼的内容，和最大可打招呼的人数，打招呼的频率
     * @param newConfig
     * @param currentConfig
     * @return
     */
    public static boolean judgeExistConfig(SayHiBean newConfig , SayHiBean currentConfig){
        if(newConfig ==null) return true;
        if(currentConfig == null) return  false;

        if( newConfig.getLatitude().equals( currentConfig.getLatitude() )
                && newConfig.getLongitude().equals( currentConfig.getLongitude() ) ){
//            currentConfig.setRate( newConfig.getRate() );
//            currentConfig.setContent(newConfig.getContent());
//            currentConfig.setMaxCount(newConfig.getMaxCount());
            return true;
        }
        return false;
    }

    /***
     * 判断当前任务是否已经完成
     * @param currentConfig
     * @return
     */
    public static boolean judgeCurrentConfigIsFinished(SayHiBean currentConfig ){
        if(currentConfig==null) return true;
        if( currentConfig.getStatus()== Constants.TASK_LOCATION_STATUS_FINISHED ) return true;
        return false;
    }
}
