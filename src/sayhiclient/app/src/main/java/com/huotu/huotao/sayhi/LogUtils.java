package com.huotu.huotao.sayhi;

import android.os.Environment;

import com.huotu.huotao.sayhi.bean.NearPersonBean;
import com.huotu.huotao.sayhi.bean.SayHiBean;

import static android.R.attr.path;

/**
 * Created by Administrator on 2017/2/23.
 */

public class LogUtils {
    public static void log(String msg ){

        if( !Environment.getExternalStorageState().equals( Environment.MEDIA_MOUNTED )  ){
            return;
        }

        String fileName = Utils.formatDate( System.currentTimeMillis(), Utils.DATE_FORMAT);
        //String path = Environment.getExternalStorageDirectory().getPath();
        //path += "/sayhi/"+ fileName + ".txt";
        //FileUtils.writeFile( path ,  msg , true );

        log(msg , fileName);
    }

    public static void log(String msg , String fileName){
        try {
            if (!BuildConfig.ISDEBUG) return;

            String nMsg = "\r\n" + msg;
            if (!Environment.getExternalStorageState().equals(Environment.MEDIA_MOUNTED)) {
                return;
            }

            String path = Environment.getExternalStorageDirectory().getPath();
            path += "/sayhi/" + fileName + ".txt";
            FileUtils.writeFile(path, nMsg , true);
        }catch (Exception ex){
            ex.printStackTrace();
        }
    }

    public static void log(NearPersonBean person){
        if(!BuildConfig.ISDEBUG)return;

        String json = WechatUtils.getGson().toJson( person );
        log(json+"\r\n");
    }

    public static void log(SayHiBean data){
        if(!BuildConfig.ISDEBUG)return;

        String json = WechatUtils.getGson().toJson(data);
        log( "------------------任务信息--------------\r\n"+json+"\r\n");

    }

    public static void log( String message , StackTraceElement[] items ){
        if(!BuildConfig.ISDEBUG)return;

        if( items==null|| items.length<1) return;
        int count = items.length;
        String error = message!=null? "" : message;
        for(int i=0;i<count;i++){
            if( !error.isEmpty() ){
                error +="\r\n";
            }
             error += items[i].toString();
        }
        String fileName = Utils.formatDate( System.currentTimeMillis() , Utils.DATE_FORMAT )+"-module";
        log(error , fileName);
    }

    public static void log(Exception ex){
        log( ex.getMessage() , ex.getStackTrace());
    }

    public static void log(Throwable throwable){
        log( throwable.getMessage() , throwable.getStackTrace());
    }

    public static void log(String messge , Throwable throwable){
        log(messge , throwable.getStackTrace());
    }
}
