package com.huotu.huotao.sayhi;

import com.huotu.huotao.sayhi.bean.BaseBean;
import com.huotu.huotao.sayhi.bean.TaskResultBeam;

import java.util.Map;

import retrofit2.Call;
import retrofit2.http.FieldMap;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.POST;

/**
 * Created by Administrator on 2017/2/21.
 */

public interface ApiDefine {

    @FormUrlEncoded
    @POST("AjaxHandler.aspx?action=adddevice")
    Call<BaseBean> addDevice(@FieldMap Map<String, String> params );

    @FormUrlEncoded
    @POST("AjaxHandler.aspx?action=gettaskinfo")
    Call<TaskResultBeam> getTaskInfo(@FieldMap Map<String,String> params);

    @FormUrlEncoded
    @POST("AjaxHandler.aspx?action=updatelocationstatus")
    Call<BaseBean> updateTaskLocationStatus(@FieldMap Map<String,String> params);

    @FormUrlEncoded
    @POST("AjaxHandler.aspx?action=updatetaskstatusrun")
    Call<BaseBean> UpdateTaskStatusRun(@FieldMap Map<String,String> params);
}
