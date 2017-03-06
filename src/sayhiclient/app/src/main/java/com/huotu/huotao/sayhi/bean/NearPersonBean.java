package com.huotu.huotao.sayhi.bean;

import java.io.Serializable;

/**
 * Created by Administrator on 2017/2/17.
 */
public class NearPersonBean implements Serializable{
    private String name;
    private String sex;
    //private String area;
    //private String distance;
    private boolean isSayHello=false;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public boolean isSayHello() {
        return isSayHello;
    }

    public void setSayHello(boolean sayHello) {
        isSayHello = sayHello;
    }

    public String getSex() {
        return sex;
    }

    public void setSex(String sex) {
        this.sex = sex;
    }

//    public String getArea() {
//        return area;
//    }
//
//    public void setArea(String area) {
//        this.area = area;
//    }

//    public String getDistance() {
//        return distance;
//    }
//
//    public void setDistance(String distance) {
//        this.distance = distance;
//    }
}
