package com.huotu.huotao.sayhi;

import android.util.Log;
import android.view.accessibility.AccessibilityNodeInfo;

import com.huotu.huotao.sayhi.bean.NearPersonBean;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import static android.content.ContentValues.TAG;

/**
 * Created by Administrator on 2017/2/17.
 */

public class Utils {

    //标准时间
    public final static String TIME_FORMAT   = "yyyy-MM-dd HH:mm:ss";

    public final static String DATE_FORMAT="yyyy-MM-dd";

    /***
     *
     * @param list
     * @param model
     * @return
     */
    public static NearPersonBean FindData(List<NearPersonBean> list , NearPersonBean model){
        if(list==null) return null;
        for(NearPersonBean item :list){
            if( item.getName().equals( model.getName() )){
                return item;
            }
        }
        return null;
    }

//    /***
//     *
//     * @param node
//     * @param className
//     * @return
//     */
//    public static AccessibilityNodeInfo findNode(AccessibilityNodeInfo node , String className){
//        if( node ==null) return null;
//        String nodeClassName = node.getClassName().toString();
//        if( nodeClassName.equals( className ) ){
//            return node;
//        }
//        int count = node.getChildCount();
//        if( count==0) return null;
//        for(int i=0;i<count;i++){
//            AccessibilityNodeInfo item = node.getChild(i);
//            AccessibilityNodeInfo find = findNode( item , className);
//            if( find == null ) continue;
//            return  find;
//        }
//        return null;
//    }

    public static String formatDate(Long currentTime){
        return formatDate( currentTime , TIME_FORMAT);
    }

    public static String formatDate(Long currentTime , String formatString ) {
        DateFormat format = null;
        try {
            format = new SimpleDateFormat(formatString);
            Date date = new Date(currentTime);
            return format.format(date);
        } catch (Exception e) {
            //发现异常时，返回当前时间
            Log.e(TAG, e.getMessage());
            return format.format(new Date());
        }
    }
}
