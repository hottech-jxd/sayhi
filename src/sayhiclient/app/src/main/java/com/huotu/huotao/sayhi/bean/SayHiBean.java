package com.huotu.huotao.sayhi.bean;

import java.io.Serializable;

/**
 * Created by Administrator on 2017/2/21.
 */
public class SayHiBean implements Serializable{
    //打招呼位置信息 纬度
    private String longitude;
    //打招呼位置信息 经度
    private String latitude;
    //打招呼内容
    private String content;
    //打招呼最大人数
    private int maxCount;
    //打招呼频率
    private int rate;
    //任务下的指定位置是否完成打招呼状态（0：未开始，1：已完成）
    private int status;
    //任务id
    private int taskid;
    //位置记录id
    private int locationid;
    //标记当前任务下的位置的操作状态是否上报成功。
    private boolean isReportStatusSuccess;
    //微信登录方式:0：手机登录，1：其他方式登录
    private String wechatloginmode;
    //微信帐号
    private String wechatusername;
    //微信密码
    private String wechatpwd;

    public String getLongitude() {
        return longitude;
    }

    public void setLongitude(String longitude) {
        this.longitude = longitude;
    }

    public String getLatitude() {
        return latitude;
    }

    public void setLatitude(String latitude) {
        this.latitude = latitude;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    public int getMaxCount() {
        return maxCount;
    }

    public void setMaxCount(int maxCount) {
        this.maxCount = maxCount;
    }

    public int getRate() {
        return rate;
    }

    public void setRate(int rate) {
        this.rate = rate;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public int getTaskid() {
        return taskid;
    }

    public void setTaskid(int taskid) {
        this.taskid = taskid;
    }

    public int getLocationid() {
        return locationid;
    }

    public void setLocationid(int locationid) {
        this.locationid = locationid;
    }

    public boolean isReportStatusSuccess() {
        return isReportStatusSuccess;
    }

    public void setReportStatusSuccess(boolean reportStatusSuccess) {
        isReportStatusSuccess = reportStatusSuccess;
    }

    public String getWechatloginmode() {
        return wechatloginmode;
    }

    public void setWechatloginmode(String wechatloginmode) {
        this.wechatloginmode = wechatloginmode;
    }

    public String getWechatusername() {
        return wechatusername;
    }

    public void setWechatusername(String wechatusername) {
        this.wechatusername = wechatusername;
    }

    public String getWechatpwd() {
        return wechatpwd;
    }

    public void setWechatpwd(String wechatpwd) {
        this.wechatpwd = wechatpwd;
    }
}
