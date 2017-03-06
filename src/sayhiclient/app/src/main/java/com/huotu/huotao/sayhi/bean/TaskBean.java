package com.huotu.huotao.sayhi.bean;

import com.huotu.huotao.sayhi.bean.LocationBean;

import java.io.Serializable;
import java.util.List;

/**
 * Created by Administrator on 2017/2/25.
 */

public class TaskBean  implements Serializable{
    private int taskid;
    private String starttime;
    private String endtime;
    private int status;
    private String sayhi;
    private String deviceno;
    private String createtime;
    private int sayhirate;
    private  int sayhimaxcount;
    private List<LocationBean> locations;

    public int getTaskid() {
        return taskid;
    }

    public void setTaskid(int taskid) {
        this.taskid = taskid;
    }

    public String getStarttime() {
        return starttime;
    }

    public void setStarttime(String starttime) {
        this.starttime = starttime;
    }

    public String getEndtime() {
        return endtime;
    }

    public void setEndtime(String endtime) {
        this.endtime = endtime;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public String getSayhi() {
        return sayhi;
    }

    public void setSayhi(String sayhi) {
        this.sayhi = sayhi;
    }

    public String getDeviceno() {
        return deviceno;
    }

    public void setDeviceno(String deviceno) {
        this.deviceno = deviceno;
    }

    public String getCreatetime() {
        return createtime;
    }

    public void setCreatetime(String createtime) {
        this.createtime = createtime;
    }

    public List<LocationBean> getLocations() {
        return locations;
    }

    public void setLocations(List<LocationBean> locations) {
        this.locations = locations;
    }

    public int getSayhirate() {
        return sayhirate;
    }

    public void setSayhirate(int sayhirate) {
        this.sayhirate = sayhirate;
    }

    public int getSayhimaxcount() {
        return sayhimaxcount;
    }

    public void setSayhimaxcount(int sayhimaxcount) {
        this.sayhimaxcount = sayhimaxcount;
    }
}
